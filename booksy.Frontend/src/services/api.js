const API_BASE_URL = 'http://localhost:5000/api'; // Or your actual .NET local port

export const api = {
    async fetch(endpoint, options = {}) {
        const defaultHeaders = {
            'Content-Type': 'application/json',
            // Add authorization headers here later if needed
        };

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
                const errorMessage = data?.title || data?.error || 'An API error occurred';
                throw new Error(errorMessage);
            }

            return data;
        } catch (error) {
            console.error(`API Request failed to ${endpoint}:`, error);
            throw error;
        }
    }
};