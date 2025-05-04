using IsaRoGaMX.CodigosPostalesMX.Interfaces;
using IsaRoGaMX.CodigosPostalesMX.Models;
using IsaRoGaMX.CodigosPostalesMX.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace IsaRoGaMX.CodigosPostalesMX.Implementations.Processors
{
    public class SqlProcessor : IProcessor
    {
        private readonly SqlProcessorSettings _settings;

        public SqlProcessor(IOptions<SqlProcessorSettings> options)
        {
            _settings = options.Value;
        }

        public void Process(DcpRow[] registros)
        {
            try
            {
                Console.WriteLine($"Registros por Procesar: {registros.Length:N}");

                int batchSize = _settings.BatchSize;
                Console.WriteLine($"Tama�o de Fragmento: {batchSize:N0}");

                int batchCount = Convert.ToInt32(Math.Ceiling(registros.Length / Convert.ToDecimal(batchSize)));
                Console.WriteLine($"Fragmentos: {batchCount:N0}");

                for (int batchIndex = 0; batchIndex < batchCount; batchIndex++)
                {
                    int offset = (batchIndex * batchSize) + (batchIndex > 0 ? 1 : 0);
                    int count = offset + batchSize;

                    count = count <= registros.Length ? (batchSize - (batchIndex == 0 ? 0 : 1)) : registros.Length - (batchIndex * batchSize) - 1;

                    Console.WriteLine($"Procesando: [{offset:N0}, {(offset + count):N0}]");

                    var dt = DtoToDataTable(new ArraySegment<DcpRow>(registros, offset, count).ToArray());

                    ExecuteProcedure(
                        "SISTEMA_RegistraColonias",
                        new SqlParameter("@colonias", SqlDbType.Structured) { Value = dt }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private DataTable DtoToDataTable(DcpRow[] registros)
        {
            var dcpTable = new DataTable("dcp")
            {
                Columns =
            {
                { "d_codigo", typeof(string) },
                { "d_asenta", typeof(string) },
                { "d_tipo_asenta", typeof(string) },
                { "d_mnpio", typeof(string) },
                { "d_estado", typeof(string) },
                { "d_ciudad", typeof(string) },
                { "d_CP", typeof(string) },
                { "c_estado", typeof(string) },
                { "c_oficina", typeof(string) },
                { "c_cp", typeof(string) },
                { "c_tipo_asenta", typeof(string) },
                { "c_mnpio", typeof(string) },
                { "id_asenta_cpcons", typeof(string) },
                { "d_zona", typeof(string) },
                { "c_cve_ciudad", typeof(string) }
            }
            };

            foreach (var registro in registros)
                dcpTable.Rows.Add(
                    registro.DCodigo,
                    registro.DAsenta,
                    registro.DTipoAsenta,
                    registro.DMnpio,
                    registro.DEstado,
                    registro.DCiudad,
                    registro.DCP,
                    registro.CEstado,
                    registro.COficina,
                    registro.CCP,
                    registro.CTipoAsenta,
                    registro.CMnpio,
                    registro.IdAsentaCpCons,
                    registro.DZona,
                    registro.CCveCiudad
                );

            return dcpTable;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_settings.ConnectionString);
        }

        private void ExecuteProcedure(string spName, params SqlParameter[] sqlParameters)
        {
            try
            {
                using var con = GetConnection();
                con.InfoMessage += Connection_InfoMessage;
                using var com = new SqlCommand(spName, con);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = _settings.Timeout;
                con.Open();
                foreach (var param in sqlParameters) com.Parameters.Add(param);

                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
