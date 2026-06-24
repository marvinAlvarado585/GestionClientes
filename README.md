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

La solución contiene **5 proyectos** (cada capa es su propio ensamblado):

```
GestionClientes.sln
├── Database/
│   └── script_basedatos.sql   Script único: BD, tablas, procedimientos y usuario admin (forma rápida)
├── GestionClientes.Database/   [SQL Server DB Project] Esquema declarativo + seed (DACPAC)
│   ├── Tables/                 Usuarios.sql · Clientes.sql · Bitacora.sql
│   ├── Stored Procedures/      1 archivo por SP (11)
│   └── Scripts/Script.PostDeployment.sql   Datos semilla
├── GestionClientes/           [Web] Presentación
│   ├── Web.config             Cadena de conexión a SQL Server
│   ├── Default.aspx           Punto de entrada (redirige a login / clientes)
│   ├── Site.Master            Plantilla común (navegación)
│   ├── Vistas/                Login, Clientes, Usuarios, Bitacora, Logout
│   └── Seguridad/SesionHelper.vb   Sesión / protección de páginas
├── BLL/                       [Biblioteca] Lógica de negocio
│   ├── ClienteBLL.vb          CRUD + regla de auditoría (registra en bitácora)
│   ├── UsuarioBLL.vb          Registro de usuarios (hash + sin duplicados)
│   ├── SeguridadBLL.vb        Autenticación (verifica el hash)
│   ├── BitacoraBLL.vb         Consulta del historial
│   └── SeguridadHelper.vb     Hashing PBKDF2 y verificación de contraseñas
├── DAL/                       [Biblioteca] Acceso a datos (invoca procedimientos almacenados)
│   ├── ConexionBD.vb
│   ├── ClienteDAL.vb · UsuarioDAL.vb · BitacoraDAL.vb
└── Models/                    [Biblioteca] Entidades del dominio
    └── Cliente.vb · Usuario.vb · RegistroBitacora.vb
```

**Arquitectura en 3 capas (proyectos separados):**
`Presentación (Web)` → `BLL` → `DAL` → `SQL Server`, con `Models` compartido.
Referencias: DAL→Models · BLL→DAL,Models · Web→BLL,Models. La presentación nunca llama al DAL
directamente: pasa siempre por la BLL, que centraliza las reglas de negocio (p. ej., registrar en la
bitácora toda modificación de clientes).

## Puesta en marcha

### 1. Base de datos

Ejecutar el script en SQL Server (SSMS o línea de comandos):

```bash
sqlcmd -S localhost -U sa -P Admin123 -i Database/script_basedatos.sql
```

Esto crea la base de datos **GestionClientesDB**, las tablas `Usuarios`, `Clientes` y `Bitacora`,
los procedimientos almacenados y el usuario administrador inicial.

> **Alternativa (proyecto de base de datos):** la solución incluye `GestionClientes.Database`, un
> **SQL Server Database Project** con el esquema declarativo (cada tabla/SP en su archivo) y un
> *Post-Deployment Script* con el seed. Al compilarlo genera un **DACPAC**; desde Visual Studio se
> puede **Publicar** (o usar *Schema Compare*) para crear/actualizar la BD. Es equivalente al script
> único, pero versionable objeto por objeto.

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
