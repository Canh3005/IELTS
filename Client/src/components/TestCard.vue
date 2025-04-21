<template>
  <router-link
    :to="{ name: 'TestDetail', params: { id: test.id }, state: { test } }"
  >
    <div
      class="bg-white rounded-lg p-4 mb-4 w-[240px] m-2 border-2 border-teal-500 hover:shadow-xl transition-shadow duration-300 cursor-pointer hover:bg-teal-50 hover:text-teal-500 hover:scale-105 transform ease-in-out hover:z-10"
    >
      <h2 class="font-bold text-lg text-teal-500 mb-2">{{ test.title }}</h2>
      <p class="text-gray-600 text-sm mb-1">
        <strong>Created At:</strong> {{ formatDate(test.createdAt) }}
      </p>
      <p class="text-gray-600 text-sm mb-1">
        <strong>Duration:</strong> {{ test.duration }} minutes
      </p>
      <p class="text-gray-600 text-sm">
        <strong>Number of Questions:</strong> {{ test.numberOfQuestions }}
      </p>
    </div>
  </router-link>
</template>

<script setup>
import { computed } from "vue";

// Props to receive test data
defineProps({
  test: {
    type: Object,
    required: true,
  },
});

// Format the date
const formatDate = (date) => {
  const options = { year: "numeric", month: "long", day: "numeric" };
  return new Date(date).toLocaleDateString(undefined, options);
};
</script>

<style scoped>
/* Add any custom styles here if needed */
</style>
