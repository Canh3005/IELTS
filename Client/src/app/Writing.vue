<script setup>
import { ref, onMounted, computed } from "vue";
import Footer from "../components/Footer.vue";
import TestCard from "../components/TestCard.vue"; // Import TestCard component
import axios from "axios";

// Reactive variables
const tests = ref([]);
const searchQuery = ref("");
const sortOption = ref("Newest");
const filteredTests = ref([]);

// Fetch data from API
const fetchTests = async () => {
  try {
    const response = await axios.get(
      `${import.meta.env.VITE_BASE_URL}/api/writing`
    );
    tests.value = response.data;
    updateFilteredTests(); // Update filtered tests after fetching
  } catch (error) {
    console.error("Error fetching tests:", error);
  }
};

// Computed property for filtered and sorted tests
const updateFilteredTests = () => {
  let filtered = tests.value.filter((test) =>
    test.title.toLowerCase().includes(searchQuery.value.toLowerCase())
  );

  if (sortOption.value === "Newest") {
    filtered.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
  } else if (sortOption.value === "Latest") {
    filtered.sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
  }

  filteredTests.value = filtered;
  console.log("Filtered Tests:", filteredTests.value);
};

onMounted(() => {
  document.title = "Writing Exam Library";
  fetchTests(); // Fetch tests when component mounts
  updateFilteredTests(); // Initialize filtered tests on mount
});
</script>

<template>
  <div class="bg-gray-100 flex flex-col items-center min-h-screen pt-10">
    <div class="flex flex-col items-center min-w-[500px] w-[1100px]">
      <h1 class="font-bold text-3xl text-teal-500 mb-4">
        Writing Exam Library
      </h1>
      <div class="mt-2 w-2/3 flex flex-row items-center justify-between">
        <div class="relative w-2/3">
          <input
            type="text"
            placeholder="Search..."
            v-model="searchQuery"
            class="border p-2 rounded-[33px] border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500 pl-4 w-full h-12"
          />
          <button
            @click="updateFilteredTests"
            class="absolute top-1 right-1 bg-teal-500 text-white p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-600"
          >
            Search
          </button>
        </div>
        <div class="w-1/3 p-2">
          <label for="sort" class="mr-2 font-semibold text-gray-600"
            >Sort by:</label
          >
          <select
            id="sort"
            v-model="sortOption"
            @change="updateFilteredTests"
            class="border p-2 rounded-[33px] border-gray-300 focus:outline-none focus:ring-2 focus:ring-teal-500 text-gray-600 h-12"
          >
            <option value="Newest" selected>Newest</option>
            <option value="Latest">Latest</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Test Cards -->
    <div
      class="mt-4 flex flex-row flex-wrap items-center min-w-[500px] w-[1060px] shadow-lg rounded-lg p-4 bg-white pl-4 pr-4"
    >
      <div v-for="test in filteredTests" :key="test.id">
        <TestCard :test="test" />
      </div>
    </div>
  </div>
</template>
