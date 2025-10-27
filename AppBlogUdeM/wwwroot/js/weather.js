const apiKey = '53fbf1c302c75a17b36ba8bb4c4a4393';

async function fetchWeather(latitude, longitude) {
    try {
        const response = await fetch(`https://api.openweathermap.org/data/2.5/weather?lat=${latitude}&lon=${longitude}&appid=${apiKey}&units=metric&lang=es`);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        if (data && data.main) {
            const temp = data.main.temp.toFixed(1);
            const locationName = data.name; // Nombre de la ciudad

            document.getElementById('weather').innerHTML = `
                <strong>${locationName}</strong><br>
                ${temp}°C
            `;
        }
    } catch (error) {
        console.error('Error al obtener el clima:', error);
        document.getElementById('weather').innerText = 'No se pudo cargar el clima';
    }
}

function getLocationAndWeather() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            position => {
                const { latitude, longitude } = position.coords;
                fetchWeather(latitude, longitude);
            },
            error => {
                console.error('Error al obtener la ubicación:', error);
                // No se muestra ningún mensaje en caso de error
            }
        );
    } else {
        console.error('Geolocalización no soportada por el navegador');
        // No se muestra ningún mensaje si la geolocalización no es soportada
    }
}

// Llama a la función para obtener la ubicación y el clima cuando el documento esté listo
document.addEventListener('DOMContentLoaded', getLocationAndWeather);