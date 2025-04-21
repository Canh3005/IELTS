<script setup>
import { onMounted } from "vue";
import Footer from "../components/Footer.vue"; // Import Footer component

onMounted(() => {
  document.title = "Home";
});
</script>

<template>
  <div
    class="flex flex-col items-center justify-center min-h-screen bg-gray-100"
  >
    <h1 class="text-4xl font-bold">Welcome to Home Page</h1>
    <p class="text-lg">This is the home page of the website.</p>
    <Footer />
  </div>
</template>
