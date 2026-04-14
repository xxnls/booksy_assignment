import { api } from './api';

export const RentalService = {
    async getAll() {
        return api.fetch('/rentalrecords');
    },

    async getById(id) {
        return api.fetch(`/rentalrecords/${id}`);
    },

    async rent(createRentalDto) {
        return api.fetch('/rentalrecords/rent', {
            method: 'POST',
            body: JSON.stringify(createRentalDto),
        });
    },

    async returnItem(id) {
        return api.fetch(`/rentalrecords/return/${id}`, {
            method: 'POST',
        });
    },

    async delete(id) {
        return api.fetch(`/rentalrecords/${id}`, {
            method: 'DELETE',
        });
    }
};