<script setup>
import { onMounted } from "vue";
import Footer from "../components/Footer.vue"; // Import Footer component

onMounted(() => {
  document.title = "Home";
});
</script>

<template>
  <div
    class="flex flex-col min-h-screen bg-gradient-to-b from-white to-teal-500 text-teal-900"
  >
    <!-- Navbar -->
    <Navbar />

    <!-- Main Content -->
    <main
      class="flex flex-col items-center justify-center flex-grow px-6 text-center"
    >
      <h1 class="text-5xl font-extrabold mb-4">Welcome to Our Website</h1>
      <p class="text-lg mb-6">
        Discover amazing content and explore our features.
      </p>
      <a
        href="#"
        class="bg-white text-teal-500 font-bold py-3 px-6 rounded-lg shadow-lg hover:bg-teal-500 hover:text-white transition"
      >
        Get Started
      </a>
    </main>

    <!-- Footer -->
    <Footer />
  </div>
</template>
