<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const email = ref('');
const password = ref('');
const error = ref('');

const handleLogin = () => {
  if (!email.value.endsWith('@booksy.com')) {
    error.value = 'Invalid domain. Please use @booksy.com';
    return;
  }
  
  error.value = '';
  // In a real app, authenticate here. For now, we bypass.
  router.push('/');
};
</script>

<template>
  <div class="login-container">
    <div class="login-card">
      <div class="logo-placeholder">[ Logo ]</div>
      <h1>Welcome back</h1>
      
      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email">Email (company domain only)</label>
          <input 
            type="email" 
            id="email" 
            v-model="email" 
            placeholder="name@booksy.com" 
            required 
          />
          <span v-if="error" class="error-message">{{ error }}</span>
        </div>
        
        <div class="form-group">
          <label for="password">Password</label>
          <input 
            type="password" 
            id="password" 
            v-model="password" 
            placeholder="••••••••" 
            required 
          />
        </div>
        
        <button type="submit" class="btn-primary">Login</button>
      </form>
    </div>
  </div>
</template>

<style scoped>
.login-container {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100vh;
  background-color: var(--bg-color);
}

.login-card {
  background: white;
  border: 1px solid var(--border-color);
  padding: 40px;
  width: 100%;
  max-width: 400px;
  text-align: center;
}

.logo-placeholder {
  width: 80px;
  height: 80px;
  background: var(--gray-light);
  border: 1px dashed var(--gray-dark);
  margin: 0 auto 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--gray-dark);
  font-size: 14px;
}

h1 {
  font-size: 24px;
  margin-bottom: 30px;
  color: var(--text-main);
}

.form-group {
  text-align: left;
  margin-bottom: 20px;
}

label {
  display: block;
  font-size: 14px;
  margin-bottom: 5px;
  color: var(--text-muted);
}

input {
  width: 100%;
  padding: 10px;
  border: 1px solid var(--border-color);
  box-sizing: border-box;
}

.error-message {
  color: #d93025;
  font-size: 12px;
  display: block;
  margin-top: 5px;
}

.btn-primary {
  width: 100%;
  padding: 12px;
  background: var(--gray-dark);
  color: white;
  border: none;
  font-size: 16px;
  cursor: pointer;
  margin-top: 10px;
}

.btn-primary:hover {
  background: black;
}
</style>