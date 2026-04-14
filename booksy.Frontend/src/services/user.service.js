import { api } from './api';

export const UserService = {
    async getAll() {
        return api.fetch('/users');
    },

    async getById(id) {
        return api.fetch(`/users/${id}`);
    },

    async create(createUserDto) {
        return api.fetch('/users', {
            method: 'POST',
            body: JSON.stringify(createUserDto),
        });
    },

    async update(id, updateUserDto) {
        return api.fetch(`/users/${id}`, {
            method: 'PUT',
            body: JSON.stringify(updateUserDto),
        });
    },

    async delete(id) {
        return api.fetch(`/users/${id}`, {
            method: 'DELETE',
        });
    }
};