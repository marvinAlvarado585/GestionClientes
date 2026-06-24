# Gestión de Clientes — Prueba Técnica FUNDAMICRO

Aplicación web desarrollada en **ASP.NET Web Forms (VB.NET, .NET Framework 4.8)** con base de datos
**SQL Server** que permite gestionar clientes con autenticación de usuarios y una bitácora de auditoría.

## Funcionalidades

- **Login de acceso:** autenticación de usuarios validada contra SQL Server. Las contraseñas se
  almacenan con **hashing PBKDF2 (HMAC-SHA256, 100.000 iteraciones)** y un *salt* único por usuario.
- **Mantenimiento de clientes:** ver, agregar, editar y eliminar registros (CRUD completo).
- **Bitácora de acciones:** cada cambio sobre los clientes (AGREGAR / EDITAR / ELIMINAR) se registra
  en una tabla separada con la acción, el Id del cliente, la fecha/hora y el usuario que la realizó.

## Tecnologías

- ASP.NET Web Forms · VB.NET · .NET Framework 4.8
- SQL Server 2022 · ADO.NET sobre **procedimientos almacenados** parametrizados (protección contra inyección SQL)
- Bootstrap 5 para la interfaz

## Estructura del proyecto

```
GestionClientes/
├── Database/
│   └── script_basedatos.sql      Script de creación de BD, tablas y usuario admin
└── GestionClientes/              Proyecto web
    ├── Web.config                Cadena de conexión a SQL Server
    ├── Default.aspx              Punto de entrada (redirige a login / clientes)
    ├── Site.Master               Plantilla común (navegación)
    ├── Vistas/                   Páginas de la aplicación
    │   ├── Login.aspx            Pantalla de inicio de sesión
    │   ├── Clientes.aspx         Pantalla principal (CRUD de clientes)
    │   ├── Usuarios.aspx         Gestión de usuarios (crear + listar)
    │   ├── Bitacora.aspx         Consulta de la bitácora
    │   └── Logout.aspx           Cierre de sesión
    ├── Modelos/                  Entidades del dominio
    │   ├── Cliente.vb
    │   ├── Usuario.vb
    │   └── RegistroBitacora.vb
    ├── BLL/                      Capa de lógica de negocio
    │   ├── ClienteBLL.vb         CRUD + regla de auditoría (registra en bitácora)
    │   ├── UsuarioBLL.vb         Registro de usuarios (hash + sin duplicados)
    │   ├── SeguridadBLL.vb       Autenticación (verifica el hash)
    │   └── BitacoraBLL.vb        Consulta del historial
    ├── DAL/                      Capa de acceso a datos (invoca procedimientos almacenados)
    │   ├── ConexionBD.vb
    │   ├── UsuarioDAL.vb
    │   ├── ClienteDAL.vb
    │   └── BitacoraDAL.vb
    └── Seguridad/
        ├── SeguridadHelper.vb    Hashing PBKDF2 y verificación de contraseñas
        └── SesionHelper.vb       Manejo de sesión / protección de páginas
```

**Arquitectura en 3 capas:** `Presentación (Vistas)` → `BLL (lógica de negocio)` → `DAL (acceso a datos)` → `SQL Server`.
Los `Modelos` se comparten entre capas. La presentación nunca llama al DAL directamente: pasa siempre por la BLL,
que centraliza las reglas de negocio (por ejemplo, registrar en la bitácora toda modificación de clientes).

## Puesta en marcha

### 1. Base de datos

Ejecutar el script en SQL Server (SSMS o línea de comandos):

```bash
sqlcmd -S localhost -U sa -P Admin123 -i Database/script_basedatos.sql
```

Esto crea la base de datos **GestionClientesDB**, las tablas `Usuarios`, `Clientes` y `Bitacora`,
y el usuario administrador inicial.

### 2. Cadena de conexión

La conexión se configura en `GestionClientes/Web.config`:

```xml
<connectionStrings>
  <add name="GestionClientesDB"
       connectionString="Server=localhost;Database=GestionClientesDB;User Id=sa;Password=Admin123;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

> Ajuste `Server`, `User Id` y `Password` según su entorno de SQL Server.

### 3. Ejecutar la aplicación

Abrir `GestionClientes.sln` en Visual Studio 2022 y presionar **F5** (IIS Express).

## Credenciales de prueba

| Usuario | Contraseña |
|---------|------------|
| `admin` | `Admin123` |

## Seguridad

- Contraseñas con hashing **PBKDF2/SHA-256** + salt (nunca en texto plano).
- Todas las consultas a la base de datos son **parametrizadas** → inmunes a inyección SQL.
- Las páginas internas exigen sesión activa; sin sesión se redirige al login.
