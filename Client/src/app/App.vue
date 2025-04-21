<script setup>
import Navbar from "../components/Navbar.vue"; // Import Navbar component
</script>

<template>
  <div>
    <!-- Navbar luôn hiển thị trên tất cả các trang -->
    <Navbar />
    <!-- router-view để hiển thị nội dung của các route -->
    <router-view />
  </div>
</template>
