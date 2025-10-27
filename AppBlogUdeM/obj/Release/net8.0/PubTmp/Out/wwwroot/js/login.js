// Función para alternar la visibilidad de la contraseña
function togglePasswordVisibility() {
    // Selecciona el campo de entrada de la contraseña y el icono del ojo
    const passwordField = document.getElementById('passwordField');
    const toggleIcon = document.getElementById('togglePasswordIcon');

    // Verifica el tipo actual del campo de contraseña
    if (passwordField.type === 'password') {
        // Si el campo está en modo "password", cámbialo a "text" para mostrar la contraseña
        passwordField.type = 'text';

        // Cambia el icono a "fa-eye-slash" para indicar que la contraseña es visible
        toggleIcon.classList.remove('fa-eye');
        toggleIcon.classList.add('fa-eye-slash');
    } else {
        // Si el campo está en modo "text", cámbialo a "password" para ocultar la contraseña
        passwordField.type = 'password';

        // Cambia el icono de vuelta a "fa-eye" para indicar que la contraseña está oculta
        toggleIcon.classList.remove('fa-eye-slash');
        toggleIcon.classList.add('fa-eye');
    }
}
