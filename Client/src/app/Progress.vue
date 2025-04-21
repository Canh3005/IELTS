<script setup>
import Footer from "../components/Footer.vue"; // Import Footer component
import { onMounted } from "vue";

onMounted(() => {
  document.title = "Progress";
});
import { ref } from "vue";
</script>

<template>
  <div class="bg-gray-100 flex flex-col items-center">
    <div>
      <div class="mt-10 flex flex-col items-center">
        <h1 class="font-bold text-3xl text-teal-500 mb-4">
          My Test Performance
        </h1>
        <div class="flex w-[1100px] min-w-[500px] justify-center">
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img
              src="../assets/AverageScore.png"
              alt="Average Score"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2">Average Score</p>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img
              src="../assets/TotalTests.png"
              alt="Total Tests"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2">Total Tests Taken</p>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img
              src="../assets/AverageTime.png"
              alt="Average Time"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2">Average Time</p>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img
              src="../assets/Accuracy.png"
              alt="Accuracy"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2">Accuracy</p>
          </div>
        </div>
      </div>
      <div class="mt-10 flex flex-col items-center">
        <h1 class="font-bold text-2xl text-teal-500 mb-4">
          My Skill Performance
        </h1>
        <div class="flex w-[1100px] min-w-[500px] justify-center">
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img
              src="../assets/Listening.png"
              alt="Average Score"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2">Listening</p>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img
              src="../assets/Reading.png"
              alt="Total Tests"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2">Reading</p>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img
              src="../assets/Speaking.png"
              alt="Average Time"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2">Speaking</p>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px] border-2 border-teal-500"
          >
            <img src="../assets/Writing.png" alt="Accuracy" class="w-20 h-20" />
            <p class="text-teal-500 text-lg mt-2">Writing</p>
          </div>
        </div>
      </div>
    </div>
    <Footer />
  </div>
</template>
