<script setup>
import { ref, onMounted } from 'vue';
import Sidebar from '../components/Sidebar.vue';
import { HardwareService } from '../services/hardware.service';
import { RentalService } from '../services/rental.service';

const hardwareList = ref([]);
const searchQuery = ref('');
const currentUser = ref(null);

const fetchHardware = async () => {
  try {
    hardwareList.value = await HardwareService.getAll();
  } catch (error) {
    console.error('Failed to fetch hardware:', error);
  }
};

const handleRent = async (hardwareId) => {
  try {
    await RentalService.rent({ hardwareId, userId: currentUser.value.id });
    alert('Device rented successfully.');
    await fetchHardware();
  } catch (error) {
    // Error is handled globally by api.js
  }
};

const handleReturn = async (rentalId) => {
  try {
    await RentalService.returnItem(rentalId);
    alert('Device returned successfully.');
    await fetchHardware();
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
  fetchHardware();
});
</script>

<template>
  <div class="layout">
    <Sidebar />
    
    <main class="content">
      <div class="search-container">
        <input 
          type="text" 
          v-model="searchQuery" 
          class="search-bar" 
          placeholder="Ask AI..."
        />
      </div>

      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>Device Name</th>
              <th>Brand</th>
              <th>Date Added</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in hardwareList" :key="item.id">
              <td>{{ item.name }}</td>
              <td>{{ item.brand }}</td>
              <td>{{ new Date(item.dateCreated).toLocaleDateString() }}</td>
              <td><span class="badge">{{ item.status }}</span></td>
              <td class="actions-cell">
                <button v-if="item.status === 'Available'" class="action-btn edit-btn" @click="handleRent(item.id)">Rent</button>
                <button v-else-if="(item.status === 'In Use' || item.status === 'InUse') && item.activeRental?.userId === currentUser?.id" class="action-btn edit-btn" @click="handleReturn(item.activeRental.id)">Return</button>
                <span v-else class="text-muted">Unavailable</span>
              </td>
            </tr>
            <tr v-if="hardwareList.length === 0">
                <td colspan="5" class="empty-state">No hardware available or failed to load.</td>
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

.search-container {
  margin-bottom: 30px;
}

.search-bar {
  width: 100%;
  padding: 15px 20px;
  font-size: 18px;
  border: 2px solid var(--border-color);
  background: white;
  box-sizing: border-box;
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