console.log('api.js is loaded');

// Setze die Basis-URL abhängig von der Umgebung
const API_BASE_URL = process.env.NODE_ENV === 'development'
    ? 'http://localhost:5000/api'  // Entwicklungsumgebung: Proxy zu localhost:5000
    : 'http://webengineering.ins.hs-anhalt.de:11032/api';  // Produktionsumgebung

// Funktion zur Generierung der vollständigen API-URL
export function getApiUrl(endpoint) {
    console.log(`${API_BASE_URL} called with:`, endpoint);
    return `${API_BASE_URL}${endpoint}`;
}