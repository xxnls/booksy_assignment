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
              <th>Category/Notes</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in hardwareList" :key="item.id">
              <td>#{{ item.id }}</td>
              <td>{{ item.name }}</td>
              <td>{{ item.notes || '-' }}</td>
              <td>{{ item.status }}</td>
              <td class="actions-cell">
                <button class="icon-btn" title="Edit" @click="handleEdit(item)">[ Edit ]</button>
                <button class="icon-btn" title="Send to Repair" @click="handleRepair(item)">[ Repair ]</button>
                <button class="icon-btn text-danger" title="Delete" @click="handleDelete(item.id)">[ Delete ]</button>
              </td>
            </tr>
            <tr v-if="hardwareList.length === 0">
                <td colspan="5" class="empty-state">No hardware available or failed to load.</td>
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

.actions-cell {
  display: flex;
  gap: 10px;
}

.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  color: var(--text-main);
  font-size: 12px;
}
.icon-btn:hover {
  text-decoration: underline;
}

.text-danger {
  color: #d93025;
}
.empty-state {
    text-align: center;
    padding: 30px;
    color: var(--text-muted);
}
</style>