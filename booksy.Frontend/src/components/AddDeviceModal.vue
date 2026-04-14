<script setup>
import { ref, watch } from 'vue';
import { HardwareService } from '../services/hardware.service';

const props = defineProps({
  isOpen: Boolean,
  editItem: Object
});

const emit = defineEmits(['close', 'saved']);

const form = ref({
  name: '',
  brand: '',
  notes: ''
});

watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    if (props.editItem) {
      form.value = {
        name: props.editItem.name,
        brand: props.editItem.brand,
        notes: props.editItem.notes || ''
      };
    } else {
      form.value = { name: '', brand: '', notes: '' };
    }
  }
});

const save = async () => {
  try {
    if (props.editItem) {
      await HardwareService.update(props.editItem.id, form.value);
    } else {
      await HardwareService.create(form.value);
    }
    emit('saved');
    form.value = { name: '', brand: '', notes: '' }; // reset
  } catch (error) {
    // Error is handled by global popup
  }
};
</script>

<template>
  <div v-if="isOpen" class="modal-overlay" @click="emit('close')">
    <div class="modal-content" @click.stop>
      <h2>{{ editItem ? 'Edit Device' : 'Add New Device' }}</h2>
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