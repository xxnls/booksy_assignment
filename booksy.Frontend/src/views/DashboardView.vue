<script setup>
import { ref, onMounted } from 'vue';
import Sidebar from '../components/Sidebar.vue';
import { HardwareService } from '../services/hardware.service';

const hardwareList = ref([]);
const searchQuery = ref('');

const fetchHardware = async () => {
  try {
    hardwareList.value = await HardwareService.getAll();
  } catch (error) {
    console.error('Failed to fetch hardware:', error);
  }
};

onMounted(() => {
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
              <td><button class="btn-action">Rent</button></td>
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

.btn-action {
  padding: 6px 12px;
  background: transparent;
  border: 1px solid var(--gray-dark);
  cursor: pointer;
}
.btn-action:hover {
  background: var(--gray-light);
}
.empty-state {
    text-align: center;
    padding: 30px;
    color: var(--text-muted);
}
</style>