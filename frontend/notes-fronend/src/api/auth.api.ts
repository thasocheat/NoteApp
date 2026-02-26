import api from "./api";
import type { User } from "@/types";


// Auth API functions
export const authApi = {
    // Register a new user
    register: (data: { username: string; email: string; password: string }) =>
        api.post<User>('/auth/register', data),

    // Login a user
    login: (data: { email: string; password: string }) =>
        api.post<User>('/auth/login', data),

    // Logout a user
    logout: () =>
        api.post('/auth/logout'),

    // Get current authenticated user
    me: () =>
        api.get<User>('/auth/me'),
}