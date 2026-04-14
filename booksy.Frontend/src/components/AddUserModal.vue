<script setup>
import { ref, watch } from 'vue';
import { UserService } from '../services/user.service';

const props = defineProps({
  isOpen: Boolean,
  editItem: Object
});

const emit = defineEmits(['close', 'saved']);

const form = ref({
  email: '',
  password: '',
  role: 'User'
});

watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    if (props.editItem) {
      form.value = {
        email: props.editItem.email,
        password: '',
        role: props.editItem.role || 'User'
      };
    } else {
      form.value = { email: '', password: '', role: 'User' };
    }
  }
});

const save = async () => {
  try {
    const payload = { ...form.value };
    if (!payload.password) delete payload.password; // Don't send empty password

    if (props.editItem) {
      await UserService.update(props.editItem.id, payload);
    } else {
      await UserService.create(payload);
    }
    emit('saved');
  } catch (error) {
    // Error handled globally
  }
};
</script>

<template>
  <div v-if="isOpen" class="modal-overlay" @click="emit('close')">
    <div class="modal-content" @click.stop>
      <h2>{{ editItem ? 'Edit User' : 'Add New User' }}</h2>
      <form @submit.prevent="save">
        
        <div class="form-group">
          <label>Email</label>
          <input type="email" v-model="form.email" required />
        </div>

        <div class="form-group">
          <label>Password</label>
          <input type="password" v-model="form.password" :placeholder="editItem ? 'Leave blank to keep unchanged' : ''" :required="!editItem" />
        </div>

        <div class="form-group">
          <label>Role</label>
          <select v-model="form.role" required>
            <option value="User">User</option>
            <option value="Admin">Admin</option>
          </select>
        </div>

        <div class="actions">
          <button type="button" class="btn-secondary" @click="emit('close')">Cancel</button>
          <button type="submit" class="btn-primary">Save</button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 30px;
  width: 400px;
  border: 1px solid var(--border-color);
}

h2 {
  margin-top: 0;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-size: 14px;
}

input, select {
  width: 100%;
  padding: 8px;
  border: 1px solid var(--border-color);
  box-sizing: border-box;
}

.actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 25px;
}

button {
  padding: 8px 15px;
  cursor: pointer;
}

.btn-secondary {
  background: transparent;
  border: 1px solid var(--border-color);
}

.btn-primary {
  background: var(--gray-dark);
  color: white;
  border: 1px solid var(--gray-dark);
}
</style>