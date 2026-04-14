<script setup>
import { ref, onMounted } from 'vue';
import Sidebar from '../components/Sidebar.vue';
import { RentalService } from '../services/rental.service';

const rentalsList = ref([]);
const currentUser = ref(null);

const fetchRentals = async () => {
  try {
    const allRentals = await RentalService.getAll();
    if (currentUser.value) {
      rentalsList.value = allRentals.filter(r => r.userId === currentUser.value.id);
    }
  } catch (error) {
    console.error('Failed to fetch rentals:', error);
  }
};

const handleReturn = async (rentalId) => {
  try {
    await RentalService.returnItem(rentalId);
    alert('Device returned successfully.');
    await fetchRentals();
  } catch (error) {
    // Error is handled globally by api.js
  }
};

onMounted(() => {
  try {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      currentUser.value = JSON.parse(userStr);
    }
  } catch(e) {}
  fetchRentals();
});
</script>

<template>
  <div class="layout">
    <Sidebar />
    
    <main class="content">
      <div class="header">
        <h1>My Rentals</h1>
      </div>

      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Device Name</th>
              <th>Rented At</th>
              <th>Returned At</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in rentalsList" :key="item.id">
              <td>#{{ item.id }}</td>
              <td>{{ item.hardwareName }}</td>
              <td>{{ new Date(item.rentedAt).toLocaleDateString() }}</td>
              <td>{{ item.returnedAt ? new Date(item.returnedAt).toLocaleDateString() : '-' }}</td>
              <td><span class="badge">{{ item.returnedAt ? 'Returned' : 'Active' }}</span></td>
              <td class="actions-cell">
                <button v-if="!item.returnedAt" class="action-btn edit-btn" @click="handleReturn(item.id)">Return</button>
                <span v-else class="text-muted">Returned</span>
              </td>
            </tr>
            <tr v-if="rentalsList.length === 0">
                <td colspan="6" class="empty-state">You have no rental history.</td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>
  </div>
</template>

<style scoped>
.layout {
  display: flex;
  height: 100vh;
  width: 100%;
}

.content {
  flex-grow: 1;
  padding: 40px;
  overflow-y: auto;
  background: var(--bg-color);
}

.header {
  margin-bottom: 30px;
}

h1 {
  margin: 0;
  font-size: 24px;
}

.table-container {
  background: white;
  border: 1px solid var(--border-color);
}

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  padding: 15px;
  text-align: left;
  border-bottom: 1px solid var(--border-color);
}

th {
  background: var(--gray-light);
  font-weight: normal;
  color: var(--text-muted);
}

.badge {
  display: inline-block;
  padding: 4px 8px;
  border: 1px solid var(--gray-dark);
  font-size: 12px;
}

.actions-cell {
  display: flex;
  gap: 10px;
  align-items: center;
}

.action-btn {
  padding: 4px 10px;
  background: white;
  border: 1px solid var(--border-color);
  border-radius: 4px;
  cursor: pointer;
  color: var(--text-main);
  font-size: 12px;
  transition: all 0.2s ease;
}

.action-btn:hover {
  background: var(--gray-light);
  border-color: var(--gray-dark);
}

.action-btn.edit-btn:hover {
  background: #e8f0fe;
  border-color: #1a73e8;
  color: #1a73e8;
}

.action-btn.repair-btn:hover {
  background: #fef7e0;
  border-color: #f29900;
  color: #f29900;
}

.text-muted {
  color: var(--text-muted);
  font-size: 14px;
  display: inline-block;
  padding: 6px 12px;
  border: 1px solid transparent;
  box-sizing: border-box;
}

.empty-state {
    text-align: center;
    padding: 30px;
    color: var(--text-muted);
}
</style>