import axios from 'axios'

// Create an Axios instance with the base URL of your API
const api = axios.create({
    // Use an environment variable for the API URL, with a fallback to localhost
    baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:7001/api',
    withCredentials: true, // Include HttpOnly cookies in requests
    
    // Set default headers for JSON requests
    headers: {
        'Content-Type': 'application/json',
    },
})

// On 401, redirect to /auth - but NOT for auth endpoints (login/register/me)
// because those 401s are expected and handled by the store/router guard.
// Triggering window.location on those would
// cause an infinite loop of 401s and redirects.
api.interceptors.response.use(
    (res) => res,
    (error) => {
        // check if the error is a 401 and the request is not to an auth endpoint
        if (error.response?.status === 401) {
            const url: string = error.config?.url ?? ''
            if (!url.includes('/auth/')){
                window.location.href = '/auth'
            }
        }
        return Promise.reject(error)
    }
)

export default api