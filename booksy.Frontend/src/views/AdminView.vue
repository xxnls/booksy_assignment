<script setup>
import { ref, onMounted } from 'vue';
import Sidebar from '../components/Sidebar.vue';
import AddDeviceModal from '../components/AddDeviceModal.vue';
import { HardwareService } from '../services/hardware.service';

const hardwareList = ref([]);
const isModalOpen = ref(false);
const itemToEdit = ref(null);

const fetchHardware = async () => {
  try {
    hardwareList.value = await HardwareService.getAll();
  } catch (error) {
    console.error('Failed to fetch hardware:', error);
  }
};

const handleDeviceSaved = () => {
  isModalOpen.value = false;
  fetchHardware();
};

const handleAdd = () => {
  itemToEdit.value = null;
  isModalOpen.value = true;
};

const handleEdit = (item) => {
  itemToEdit.value = item;
  isModalOpen.value = true;
};

const handleRepair = async (item) => {
  if(confirm(`Send ${item.name} to repair?`)) {
    try {
      await HardwareService.update(item.id, { status: 'UnderMaintenance' });
      await fetchHardware();
    } catch(e) {}
  }
};

const handleDelete = async (id) => {
    if(confirm('Are you sure you want to delete this device?')) {
        try {
            await HardwareService.delete(id);
            await fetchHardware();
        } catch(e) {}
    }
}

onMounted(() => {
  fetchHardware();
});
</script>

<template>
  <div class="layout">
    <Sidebar />
    
    <main class="content">
      <div class="header">
        <h1>Hardware Management</h1>
        <button class="btn-primary" @click="handleAdd">Add New Device</button>
      </div>

      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Device Name</th>
              <th>Brand</th>
              <th>Notes</th>
              <th>Purchase Date</th>
              <th>Date Added</th>
              <th>History</th>
              <th>Assigned To</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in hardwareList" :key="item.id">
              <td>#{{ item.id }}</td>
              <td>{{ item.name }}</td>
              <td>{{ item.brand }}</td>
              <td>{{ item.notes || '-' }}</td>
              <td>{{ item.purchaseDate ? new Date(item.purchaseDate).toLocaleDateString() : '-' }}</td>
              <td>{{ new Date(item.dateCreated).toLocaleDateString() }}</td>
              <td>{{ item.history || '-' }}</td>
              <td>{{ item.activeRental?.userEmail || '-' }}</td>
              <td><span class="badge">{{ item.status }}</span></td>
              <td class="actions-cell">
                <div class="action-buttons">
                  <button class="action-btn edit-btn" title="Edit" @click="handleEdit(item)">Edit</button>
                  <button class="action-btn repair-btn" title="Send to Repair" @click="handleRepair(item)">Repair</button>
                  <button class="action-btn delete-btn" title="Delete" @click="handleDelete(item.id)">Delete</button>
                </div>
              </td>
            </tr>
            <tr v-if="hardwareList.length === 0">
                <td colspan="10" class="empty-state">No hardware available or failed to load.</td>
            </tr>
          </tbody>
        </table>
      </div>

      <AddDeviceModal 
        :isOpen="isModalOpen" 
        :editItem="itemToEdit"
        @close="isModalOpen = false"
        @saved="handleDeviceSaved"
      />
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
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

h1 {
  margin: 0;
  font-size: 24px;
}

.btn-primary {
  padding: 10px 20px;
  background: var(--gray-dark);
  color: white;
  border: none;
  cursor: pointer;
}

.table-container {
  background: white;
  border: 1px solid var(--border-color);
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  white-space: nowrap;
}

th, td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid var(--border-color);
  vertical-align: middle;
}

th {
  background: var(--gray-light);
  font-weight: 500;
  color: var(--text-muted);
}

.badge {
  display: inline-block;
  padding: 4px 8px;
  border: 1px solid var(--gray-dark);
  font-size: 12px;
  border-radius: 12px;
  background-color: var(--gray-light);
}

.actions-cell {
  width: 1%;
  white-space: nowrap;
}

.action-buttons {
  display: flex;
  gap: 8px;
  justify-content: flex-start;
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

.action-btn.delete-btn {
  color: #d93025;
  border-color: #f1a8a2;
}

.action-btn.delete-btn:hover {
  background: #fce8e6;
  border-color: #d93025;
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

.empty-state {
    text-align: center;
    padding: 30px;
    color: var(--text-muted);
}
</style>