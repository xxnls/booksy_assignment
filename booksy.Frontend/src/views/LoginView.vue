<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { UserService } from '../services/user.service';

const router = useRouter();
const email = ref('');
const password = ref('');
const error = ref('');

const handleLogin = async () => {
  error.value = '';
  
  try {
    const response = await UserService.login(
      { email: email.value, password: password.value },
      { hideErrorPopup: true }
    );
    if (response && response.token) {
      localStorage.setItem('token', response.token);
      localStorage.setItem('user', JSON.stringify(response.user));
      router.push('/');
    }
  } catch (err) {
    error.value = err.message || 'Login failed. Please check your credentials.';
  }
};
</script>

<template>
  <div class="login-container">
    <div class="login-card">
      <div class="logo-icon">
        <svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M20 16V7a2 2 0 0 0-2-2H6a2 2 0 0 0-2 2v9m16 0H4m16 0 1.28 2.55a1 1 0 0 1-.9 1.45H3.62a1 1 0 0 1-.9-1.45L4 16"></path>
        </svg>
      </div>
      <h1>Welcome back</h1>
      
      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email">Email</label>
          <input 
            type="email" 
            id="email" 
            v-model="email" 
            placeholder="name@company.com" 
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

.logo-icon {
  width: 80px;
  height: 80px;
  background: var(--gray-light);
  border: 1px solid var(--gray-dark);
  border-radius: 50%;
  margin: 0 auto 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--gray-dark);
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