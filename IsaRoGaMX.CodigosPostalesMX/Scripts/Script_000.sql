CREATE TABLE DireccionesCodigosPostales (
  d_codigo         VARCHAR(5) NOT NULL,
  d_asenta         VARCHAR(60),
  d_tipo_asenta    VARCHAR(40),
  d_mnpio          VARCHAR(50),
  d_estado         VARCHAR(35),
  d_ciudad         VARCHAR(50),
  d_CP             VARCHAR(5),
  c_estado         VARCHAR(2),
  c_oficina        VARCHAR(5),
  c_cp             VARCHAR(5),
  c_tipo_asenta    VARCHAR(2),
  c_mnpio          VARCHAR(3),
  id_asenta_cpcons VARCHAR(4) NOT NULL,
  d_zona           VARCHAR(40),
  c_cve_ciudad     VARCHAR(2),
  CONSTRAINT PK_DireccionesCodigosPostales
    PRIMARY KEY (id_asenta_cpcons, d_codigo)
  )
GO

CREATE TYPE dcp AS TABLE
(
  d_codigo         VARCHAR(5),
  d_asenta         VARCHAR(60),
  d_tipo_asenta    VARCHAR(40),
  d_mnpio          VARCHAR(50),
  d_estado         VARCHAR(35),
  d_ciudad         VARCHAR(50),
  d_CP             VARCHAR(5),
  c_estado         VARCHAR(2),
  c_oficina        VARCHAR(5),
  c_cp             VARCHAR(5),
  c_tipo_asenta    VARCHAR(2),
  c_mnpio          VARCHAR(3),
  id_asenta_cpcons VARCHAR(4),
  d_zona           VARCHAR(40),
  c_cve_ciudad     VARCHAR(2)
  )
GO

CREATE OR ALTER PROCEDURE [dbo].[SISTEMA_RegistraColonias]
  @colonias dcp READONLY
AS
BEGIN
  SET NOCOUNT ON;
  
  DECLARE @ColoniasRecibidas INT = 0
    , @ColoniasARegistrar INT = 0
  
  DECLARE @listaColonias TABLE (
    d_codigo         VARCHAR(5),
    d_asenta         VARCHAR(60),
    d_tipo_asenta    VARCHAR(40),
    d_mnpio          VARCHAR(50),
    d_estado         VARCHAR(35),
    d_ciudad         VARCHAR(50),
    d_CP             VARCHAR(5),
    c_estado         VARCHAR(2),
    c_oficina        VARCHAR(5),
    c_cp             VARCHAR(5),
    c_tipo_asenta    VARCHAR(2),
    c_mnpio          VARCHAR(3),
    id_asenta_cpcons VARCHAR(4),
    d_zona           VARCHAR(40),
    c_cve_ciudad     VARCHAR(2)
  )
  
  INSERT INTO @listaColonias (
      d_codigo, d_asenta, d_tipo_asenta, d_mnpio, d_estado,
      d_ciudad, d_CP, c_estado, c_oficina, c_cp,
      c_tipo_asenta, c_mnpio, id_asenta_cpcons, d_zona, c_cve_ciudad
  ) SELECT d_codigo, d_asenta, d_tipo_asenta, d_mnpio, d_estado,
         d_ciudad, d_CP, c_estado, c_oficina, c_cp,
         c_tipo_asenta, c_mnpio, id_asenta_cpcons, d_zona, c_cve_ciudad
  FROM @colonias

  SELECT @ColoniasRecibidas = COUNT(*) FROM @listaColonias
  PRINT 'Lista con ' + CONVERT(VARCHAR, @ColoniasRecibidas) + ' registros.'

  -- Se eliminan las colonias ya registradas
  DELETE cols
  FROM @listaColonias cols
  INNER JOIN DireccionesCodigosPostales dcp
  ON cols.d_codigo = dcp.d_codigo
  AND cols.id_asenta_cpcons = dcp.id_asenta_cpcons

  SELECT @ColoniasARegistrar = COUNT(*) FROM @listaColonias
  PRINT 'Colonias a insertar: ' + CONVERT(VARCHAR, @ColoniasARegistrar)

  -- Insertar los registros que estan en el parametro pero no en la tabla
  INSERT INTO DireccionesCodigosPostales (
      d_codigo, d_asenta, d_tipo_asenta, d_mnpio, d_estado,
      d_ciudad, d_CP, c_estado, c_oficina, c_cp,
      c_tipo_asenta, c_mnpio, id_asenta_cpcons, d_zona, c_cve_ciudad
  )
  SELECT d_codigo, d_asenta, d_tipo_asenta, d_mnpio, d_estado,
         d_ciudad, d_CP, c_estado, c_oficina, c_cp,
         c_tipo_asenta, c_mnpio, id_asenta_cpcons, d_zona, c_cve_ciudad
  FROM @listaColonias
END
GO