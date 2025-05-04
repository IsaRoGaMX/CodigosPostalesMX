# 🇲🇽 Actualizador de Códigos Postales de México (.NET Console App)

Aplicación de consola desarrollada en .NET que descarga la lista completa de códigos postales de México desde correos de méxico e importa las colonias agregadas recientemente a una base de datos SQL Server.

## 🚀 Características

- Descarga automatizada de códigos postales de México.
- Registro automático en la base de datos SQL Server (Revisar la carpeta Scripts)

## 📦 Requisitos

- [.NET SDK 8.0 o superior](https://dotnet.microsoft.com/)
- Acceso a Internet (para la descarga de datos)

## 🛠️ Uso
1. Ejecutar el archivo Script_000.sql para crear la tabla y el SP para registrar las colonias
2. Crear una tarea programada en el servidor para que ejecute todos los dias la aplicación.

## 📚 Fuente de Datos
[Descarga de Códigos Postales](https://www.correosdemexico.gob.mx/SSLServicios/ConsultaCP/CodigoPostal_Exportar.aspx)

## 📝 TODO

- [ ] Agregar logging
- [ ] Habilitar solo la descarga seleccionando el formato de salida por parámetros (CSV, JSON, etc.)
- [ ] Validar y manejar errores de red o formatos corruptos
- [ ] Implementar pruebas unitarias para el proceso de descarga

Desarrollado en .NET por [@IsaRoGaMX](https://github.com/IsaRoGaMX).
