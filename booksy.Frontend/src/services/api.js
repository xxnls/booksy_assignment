const API_BASE_URL = 'https://localhost:7227/api'; // Or your actual .NET local port

export const api = {
    async fetch(endpoint, options = {}) {
        const token = localStorage.getItem('token');
        const defaultHeaders = {
            'Content-Type': 'application/json',
        };

        if (token) {
            defaultHeaders['Authorization'] = `Bearer ${token}`;
        }

        const config = {
            ...options,
            headers: {
                ...defaultHeaders,
                ...options.headers,
            },
        };

        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
            
            // Check for NoContent before attempting to parse JSON
            if (response.status === 204) {
                 return { ok: true };
            }

            const data = await response.json().catch(() => null);

            if (!response.ok) {
                const errorMessage = data?.title || data?.error || data || 'An API error occurred';
                throw new Error(errorMessage);
            }

            return data;
        } catch (error) {
            console.error(`API Request failed to ${endpoint}:`, error);
            throw error;
        }
    }
};