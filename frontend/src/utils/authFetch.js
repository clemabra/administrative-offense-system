// replaces fetch and adds authorization header + token to the request
export function authFetch(url, options = {}) {
    const token = localStorage.getItem('token');
    if (!token) throw new Error('No token found');

    options.headers = {
        ...options.headers,
        'Authorization': `Bearer ${token}`
    };

    return fetch(url, options);
}