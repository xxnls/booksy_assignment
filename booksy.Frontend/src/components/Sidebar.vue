<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const isAdmin = ref(false);

onMounted(() => {
  try {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      const user = JSON.parse(userStr);
      isAdmin.value = user.role === 'Admin';
    }
  } catch (e) {}
});

const handleLogout = () => {
  localStorage.removeItem('token');
  localStorage.removeItem('user');
  router.push('/login');
};
</script>

<template>
  <aside class="sidebar">
    <div class="logo-icon">
      <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M20 16V7a2 2 0 0 0-2-2H6a2 2 0 0 0-2 2v9m16 0H4m16 0 1.28 2.55a1 1 0 0 1-.9 1.45H3.62a1 1 0 0 1-.9-1.45L4 16"></path>
      </svg>
    </div>
    <nav>
      <router-link to="/" class="nav-link" active-class="active">Hardware List</router-link>
      <router-link to="/rentals" class="nav-link" active-class="active">My Rentals</router-link>
      <router-link v-if="isAdmin" to="/admin" class="nav-link" active-class="active">Admin Panel</router-link>
      <router-link v-if="isAdmin" to="/users" class="nav-link" active-class="active">Users Panel</router-link>
    </nav>
    <button @click="handleLogout" class="btn-logout">Logout</button>
  </aside>
</template>

<style scoped>
.sidebar {
  width: 250px;
  border-right: 1px solid var(--border-color);
  display: flex;
  flex-direction: column;
  padding: 20px;
  background: white;
  height: 100vh;
  box-sizing: border-box;
}

.logo-icon {
  height: 50px;
  background: var(--gray-light);
  border: 1px solid var(--gray-dark);
  border-radius: 5px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 40px;
  color: var(--gray-dark);
}

nav {
  flex-grow: 1;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.nav-link {
  padding: 10px 15px;
  text-decoration: none;
  color: var(--text-main);
  border: 1px solid transparent;
}

.nav-link:hover {
  background: var(--gray-light);
}

.nav-link.active {
  background: var(--gray-light);
  border-color: var(--border-color);
  font-weight: bold;
}

.btn-logout {
  padding: 10px;
  background: transparent;
  border: 1px solid var(--border-color);
  cursor: pointer;
  color: var(--text-main);
}
.btn-logout:hover {
  background: var(--gray-light);
}
</style>