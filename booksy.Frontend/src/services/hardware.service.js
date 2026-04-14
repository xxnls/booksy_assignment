import { api } from './api';

export const HardwareService = {
    async getAll() {
        return api.fetch('/hardware');
    },

    async getById(id) {
        return api.fetch(`/hardware/${id}`);
    },

    async create(hardwareDto) {
        return api.fetch('/hardware', {
            method: 'POST',
            body: JSON.stringify(hardwareDto),
        });
    },

    async update(id, updateHardwareDto) {
        return api.fetch(`/hardware/${id}`, {
            method: 'PUT',
            body: JSON.stringify(updateHardwareDto),
        });
    },

    async delete(id) {
        return api.fetch(`/hardware/${id}`, {
            method: 'DELETE',
        });
    }
};