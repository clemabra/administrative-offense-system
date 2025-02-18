/*
 * OPAS - API Configuration
 * Copyright (C) 2025 Ims2425Bruh
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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