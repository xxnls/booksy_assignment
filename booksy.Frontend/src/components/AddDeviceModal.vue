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
  purchaseDate: '',
  status: 'Available',
  notes: '',
  history: '',
  assignedTo: ''
});

watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    if (props.editItem) {
      form.value = {
        name: props.editItem.name,
        brand: props.editItem.brand,
        purchaseDate: props.editItem.purchaseDate || '',
        status: props.editItem.status || 'Available',
        notes: props.editItem.notes || '',
        history: props.editItem.history || '',
        assignedTo: '' // Not applicable for edit
      };
    } else {
      form.value = { name: '', brand: '', purchaseDate: '', status: 'Available', notes: '', history: '', assignedTo: '' };
    }
  }
});

const save = async () => {
  try {
    const payload = { ...form.value };
    if (!payload.purchaseDate) delete payload.purchaseDate;
    if (!payload.assignedTo) delete payload.assignedTo;

    if (props.editItem) {
      delete payload.assignedTo; // Ensure assignedTo isn't sent on update
      await HardwareService.update(props.editItem.id, payload);
    } else {
      await HardwareService.create(payload);
    }
    emit('saved');
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
        <div class="form-row">
          <div class="form-group">
            <label>Name</label>
            <input type="text" v-model="form.name" required />
          </div>
          <div class="form-group">
            <label>Brand</label>
            <input type="text" v-model="form.brand" required />
          </div>
        </div>
        
        <div class="form-row">
          <div class="form-group">
            <label>Purchase Date</label>
            <input type="date" v-model="form.purchaseDate" />
          </div>
          <div class="form-group">
            <label>Status</label>
            <select v-model="form.status" required>
              <option value="Available">Available</option>
              <option value="InUse">In Use</option>
              <option value="UnderMaintenance">Under Maintenance</option>
              <option value="Retired">Retired</option>
            </select>
          </div>
        </div>

        <div class="form-group">
          <label>Category / Notes</label>
          <input type="text" v-model="form.notes" />
        </div>
        
        <div class="form-group">
          <label>History</label>
          <textarea v-model="form.history" rows="3"></textarea>
        </div>

        <div v-if="!editItem" class="form-group">
          <label>Assign To (User Email)</label>
          <input type="email" v-model="form.assignedTo" placeholder="Optional" />
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
  width: 500px;
  border: 1px solid var(--border-color);
  max-height: 90vh;
  overflow-y: auto;
}

h2 {
  margin-top: 0;
  margin-bottom: 20px;
}

.form-row {
  display: flex;
  gap: 15px;
}

.form-row .form-group {
  flex: 1;
}

.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-size: 14px;
}

input, select, textarea {
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