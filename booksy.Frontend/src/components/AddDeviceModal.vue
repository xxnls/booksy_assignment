<script setup>
import { ref } from 'vue';
import { HardwareService } from '../services/hardware.service';

const props = defineProps({
  isOpen: Boolean
});

const emit = defineEmits(['close', 'saved']);

const form = ref({
  name: '',
  brand: '',
  notes: ''
});

const save = async () => {
  try {
    await HardwareService.create(form.value);
    emit('saved');
    form.value = { name: '', brand: '', notes: '' }; // reset
  } catch (error) {
    console.error('Failed to create device:', error);
    alert('Failed to save. Check console.');
  }
};
</script>

<template>
  <div v-if="isOpen" class="modal-overlay" @click="emit('close')">
    <div class="modal-content" @click.stop>
      <h2>Add New Device</h2>
      <form @submit.prevent="save">
        <div class="form-group">
          <label>Name</label>
          <input type="text" v-model="form.name" required />
        </div>
        <div class="form-group">
          <label>Brand</label>
          <input type="text" v-model="form.brand" required />
        </div>
        <div class="form-group">
          <label>Notes / Category</label>
          <input type="text" v-model="form.notes" />
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

input {
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