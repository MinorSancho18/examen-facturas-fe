# Cómo Agregar un Colaborador a un Repositorio de GitHub

Esta guía explica cómo agregar colaboradores a tu repositorio de GitHub.

## Requisitos Previos

- Debes ser el propietario del repositorio o tener permisos de administrador
- El colaborador debe tener una cuenta de GitHub

## Método 1: A través de la Interfaz Web de GitHub

### Pasos:

1. **Navega a tu repositorio**
   - Ve a https://github.com/TU_USUARIO/TU_REPOSITORIO

2. **Accede a la configuración del repositorio**
   - Haz clic en la pestaña "Settings" (Configuración) en la parte superior del repositorio

3. **Abre la sección de colaboradores**
   - En el menú lateral izquierdo, haz clic en "Collaborators" (Colaboradores)
   - Si es un repositorio privado, puede estar en "Collaborators and teams" (Colaboradores y equipos)

4. **Autenticación**
   - Es posible que GitHub te pida confirmar tu contraseña por seguridad

5. **Agregar un nuevo colaborador**
   - Haz clic en el botón "Add people" (Agregar personas)
   - En el cuadro de búsqueda, escribe el nombre de usuario de GitHub del colaborador
   - Selecciona el usuario correcto de la lista de resultados

6. **Enviar la invitación**
   - Haz clic en "Add [nombre de usuario] to this repository"
   - Se enviará una invitación por correo electrónico al colaborador

7. **El colaborador debe aceptar la invitación**
   - El colaborador recibirá un correo electrónico de GitHub
   - Debe hacer clic en el enlace de la invitación y aceptarla
   - También puede aceptarla desde https://github.com/notifications

## Método 2: A través de la Línea de Comandos (GitHub CLI)

Si tienes instalado [GitHub CLI](https://cli.github.com/), puedes agregar colaboradores desde la terminal:

### Instalación de GitHub CLI (si no lo tienes):

```bash
# En macOS
brew install gh

# En Windows (con Chocolatey)
choco install gh

# En Linux (Debian/Ubuntu)
curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg | sudo dd of=/usr/share/keyrings/githubcli-archive-keyring.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" | sudo tee /etc/apt/sources.list.d/github-cli.list > /dev/null
sudo apt update
sudo apt install gh
```

### Autenticación:

```bash
gh auth login
```

### Agregar colaborador:

```bash
# Sintaxis básica
gh api repos/PROPIETARIO/REPOSITORIO/collaborators/NOMBRE_USUARIO --method=PUT

# Ejemplo
gh api repos/MinorSancho18/examen-facturas-fe/collaborators/usuario-colaborador --method=PUT
```

## Niveles de Permisos

Cuando agregas un colaborador, puedes asignar diferentes niveles de permisos:

- **Read** (Lectura): Puede ver y clonar el repositorio
- **Triage**: Puede leer y gestionar issues y pull requests
- **Write** (Escritura): Puede leer, clonar y push al repositorio
- **Maintain** (Mantener): Puede gestionar el repositorio sin acceso a acciones sensibles
- **Admin** (Administrador): Acceso completo al repositorio, incluyendo configuración

### Para repositorios públicos:

En repositorios públicos, los colaboradores generalmente reciben permisos de **Write** por defecto.

### Para repositorios privados:

Puedes especificar el nivel de permiso al agregar al colaborador.

## Verificar Colaboradores Actuales

### A través de la web:

1. Ve a Settings → Collaborators
2. Verás una lista de todos los colaboradores actuales

### A través de GitHub CLI:

```bash
gh api repos/PROPIETARIO/REPOSITORIO/collaborators
```

## Eliminar un Colaborador

### A través de la web:

1. Ve a Settings → Collaborators
2. Busca al colaborador que deseas eliminar
3. Haz clic en el botón "Remove" (Eliminar) junto a su nombre

### A través de GitHub CLI:

```bash
gh api repos/PROPIETARIO/REPOSITORIO/collaborators/NOMBRE_USUARIO --method=DELETE
```

## Notas Importantes

- Los colaboradores tienen acceso completo al código del repositorio
- Pueden hacer push directamente a las ramas (a menos que estén protegidas)
- No pueden modificar la configuración del repositorio (a menos que tengan permisos de Admin)
- Las invitaciones expiran después de 7 días si no son aceptadas
- Puedes revocar el acceso de un colaborador en cualquier momento

## Para Organizaciones

Si tu repositorio pertenece a una organización de GitHub:

1. Ve a la página de la organización
2. Haz clic en "Teams" (Equipos)
3. Crea o selecciona un equipo
4. Agrega miembros al equipo
5. Asigna el equipo al repositorio con los permisos apropiados

Esto permite una mejor gestión de permisos a escala.

## Recursos Adicionales

- [Documentación oficial de GitHub sobre colaboradores](https://docs.github.com/en/account-and-profile/setting-up-and-managing-your-personal-account-on-github/managing-access-to-your-personal-repositories/inviting-collaborators-to-a-personal-repository)
- [GitHub CLI Documentation](https://cli.github.com/manual/)
- [Gestión de equipos en organizaciones](https://docs.github.com/en/organizations/organizing-members-into-teams)

---

## Ejemplo Práctico

Para este repositorio (`examen-facturas-fe`), los pasos serían:

1. El propietario (MinorSancho18) va a https://github.com/MinorSancho18/examen-facturas-fe/settings/access
2. Hace clic en "Add people"
3. Escribe el nombre de usuario del colaborador
4. Selecciona el usuario y envía la invitación
5. El colaborador acepta la invitación desde su correo o desde GitHub

¡Listo! El colaborador ahora puede trabajar en el repositorio.
