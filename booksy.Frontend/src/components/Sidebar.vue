<script setup>
import { useRouter } from 'vue-router';

const router = useRouter();

const handleLogout = () => {
  router.push('/login');
};
</script>

<template>
  <aside class="sidebar">
    <div class="logo">[ Logo ]</div>
    <nav>
      <router-link to="/" class="nav-link" active-class="active">Hardware List</router-link>
      <router-link to="/rentals" class="nav-link" active-class="active">My Rentals</router-link>
      <router-link to="/admin" class="nav-link" active-class="active">Admin Panel</router-link>
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

.logo {
  height: 50px;
  background: var(--gray-light);
  border: 1px dashed var(--gray-dark);
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 40px;
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