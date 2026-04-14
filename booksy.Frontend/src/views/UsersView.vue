<script setup>
import { ref, onMounted } from 'vue';
import Sidebar from '../components/Sidebar.vue';
import AddUserModal from '../components/AddUserModal.vue';
import { UserService } from '../services/user.service';

const usersList = ref([]);
const isModalOpen = ref(false);
const itemToEdit = ref(null);

const fetchUsers = async () => {
  try {
    usersList.value = await UserService.getAll();
  } catch (error) {
    console.error('Failed to fetch users:', error);
  }
};

const handleUserSaved = () => {
  isModalOpen.value = false;
  fetchUsers();
};

const handleAdd = () => {
  itemToEdit.value = null;
  isModalOpen.value = true;
};

const handleEdit = (item) => {
  itemToEdit.value = item;
  isModalOpen.value = true;
};

const handleDelete = async (id) => {
    if(confirm('Are you sure you want to delete this user?')) {
        try {
            await UserService.delete(id);
            await fetchUsers();
        } catch(e) {}
    }
}

onMounted(() => {
  fetchUsers();
});
</script>

<template>
  <div class="layout">
    <Sidebar />
    
    <main class="content">
      <div class="header">
        <h1>User Management</h1>
        <button class="btn-primary" @click="handleAdd">Add New User</button>
      </div>

      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Email</th>
              <th>Role</th>
              <th>Date Created</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in usersList" :key="item.id">
              <td>#{{ item.id }}</td>
              <td>{{ item.email }}</td>
              <td><span class="badge">{{ item.role }}</span></td>
              <td>{{ new Date(item.dateCreated).toLocaleDateString() }}</td>
              <td class="actions-cell">
                <div class="action-buttons">
                  <button class="action-btn edit-btn" title="Edit" @click="handleEdit(item)">Edit</button>
                  <button class="action-btn delete-btn" title="Delete" @click="handleDelete(item.id)">Delete</button>
                </div>
              </td>
            </tr>
            <tr v-if="usersList.length === 0">
                <td colspan="5" class="empty-state">No users available or failed to load.</td>
            </tr>
          </tbody>
        </table>
      </div>

      <AddUserModal 
        :isOpen="isModalOpen" 
        :editItem="itemToEdit"
        @close="isModalOpen = false"
        @saved="handleUserSaved"
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

.empty-state {
    text-align: center;
    padding: 30px;
    color: var(--text-muted);
}
</style>