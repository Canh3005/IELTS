<script setup>
import { onMounted, ref } from "vue";

const averageScore = ref("-");
const totalTests = ref("-");
const averageTime = ref("-");
const averageAccuracy = ref("-");

const userId = localStorage.getItem("userid"); // hoặc lấy userId theo cách bạn lưu
const activeTab = ref("Listening"); // Default active tab

const skillStats = ref({
  Listening: {},
  Reading: {},
  Speaking: {},
  Writing: {},
});

if (!userId) {
  console.error("User ID not found in localStorage.");
}

onMounted(async () => {
  document.title = "Progress";
  if (!userId) return;

  try {
    const res = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/progress/${userId}`
    );
    const data = await res.json();
    if (data && !data.message) {
      averageScore.value = data.averageScore;
      totalTests.value = data.totalTests;
      averageTime.value = data.averageTime;
      averageAccuracy.value = data.averageAccuracy;
    }

    const resSkill = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/progress/skill/${userId}`
    );
    const dataSkill = await resSkill.json();
    console.log("Skill Data:", dataSkill);
    if (Array.isArray(dataSkill)) {
      dataSkill.forEach((item) => {
        console.log("Processing item:", item);
        skillStats.value[item.testType] = item;
      });
    }
    console.log("Skill Stats:", skillStats.value);
  } catch (e) {
    console.error("Failed to fetch progress:", e);
  }
});
</script>

<template>
  <div class="bg-gray-100 flex flex-col items-center min-h-screen">
    <div>
      <div class="mt-10 flex flex-col items-center">
        <h1 class="font-bold text-3xl text-teal-500 mb-4">
          My Test Performance
        </h1>
        <div class="flex w-[1100px] min-w-[500px] justify-center">
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px]"
          >
            <img
              src="../assets/AverageScore.png"
              alt="Average Score"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg font-semibold mt-2">
              Average Score
            </p>
            <span class="text-2xl font-semibold mt-1">{{ averageScore }}</span>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px]"
          >
            <img
              src="../assets/TotalTests.png"
              alt="Total Tests"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2 font-semibold">
              Total Tests Taken
            </p>
            <span class="text-2xl font-semibold mt-1">{{ totalTests }}</span>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px]"
          >
            <img
              src="../assets/AverageTime.png"
              alt="Average Time"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2 font-semibold">Average Time</p>
            <span class="text-2xl font-semibold mt-1">{{ averageTime }}</span>
          </div>
          <div
            class="flex flex-col items-center bg-white shadow-lg rounded-lg p-4 m-2 w-[240px]"
          >
            <img
              src="../assets/Accuracy.png"
              alt="Accuracy"
              class="w-20 h-20"
            />
            <p class="text-teal-500 text-lg mt-2 font-semibold">Accuracy</p>
            <span class="text-2xl font-semibold mt-1"
              >{{ averageAccuracy }}%
            </span>
          </div>
        </div>
      </div>
      <div class="ml-10 mt-10 w-full">
        <div class="mt-10 flex justify-left items-center">
          <button
            @click="activeTab = 'Listening'"
            :class="
              activeTab === 'Listening'
                ? 'mr-4 rounded-full bg-teal-500 text-white p-2 border border-teal-500 cursor-pointer'
                : 'mr-4 rounded-full bg-white text-teal-500 p-2 hover:bg-teal-500 hover:text-white border border-teal-500 cursor-pointer'
            "
          >
            Listening
          </button>
          <button
            @click="activeTab = 'Reading'"
            :class="
              activeTab === 'Reading'
                ? 'mr-4 rounded-full bg-teal-500 text-white p-2 border border-teal-500 cursor-pointer'
                : 'mr-4 rounded-full bg-white text-teal-500 p-2 hover:bg-teal-500 hover:text-white border border-teal-500 cursor-pointer'
            "
          >
            Reading
          </button>
          <button
            @click="activeTab = 'Speaking'"
            :class="
              activeTab === 'Speaking'
                ? 'mr-4 rounded-full bg-teal-500 text-white p-2 border border-teal-500 cursor-pointer'
                : 'mr-4 rounded-full bg-white text-teal-500 p-2 hover:bg-teal-500 hover:text-white border border-teal-500 cursor-pointer'
            "
          >
            Speaking
          </button>
          <button
            @click="activeTab = 'Writing'"
            :class="
              activeTab === 'Writing'
                ? 'mr-4 rounded-full bg-teal-500 text-white p-2 border border-teal-500 cursor-pointer'
                : 'mr-4 rounded-full bg-white text-teal-500 p-2 hover:bg-teal-500 hover:text-white border border-teal-500 cursor-pointer'
            "
          >
            Writing
          </button>
        </div>
        <div>
          <div class="flex flex-row mt-5">
            <div
              class="flex flex-col items-center bg-white shadow-lg rounded-lg p-2 m-2 w-[200px] shadow-md"
            >
              <p class="text-teal-500 text-lg mt-1 font-semibold">
                Average Score
              </p>
              <span class="text-2xl font-semibold mt-1">{{
                skillStats[activeTab].averageScore
              }}</span>
            </div>
            <div
              class="flex flex-col items-center bg-white shadow-lg rounded-lg p-2 m-2 w-[200px] shadow-md"
            >
              <p class="text-teal-500 text-lg mt-1 font-semibold">
                Total Tests Taken
              </p>
              <span class="text-2xl font-semibold mt-1">{{
                skillStats[activeTab].totalTestTaken
              }}</span>
            </div>
            <div
              class="flex flex-col items-center bg-white shadow-lg rounded-lg p-2 m-2 w-[200px] shadow-md"
            >
              <p class="text-teal-500 text-lg mt-1 font-semibold">
                Average Time
              </p>
              <span class="text-2xl font-semibold mt-1">{{
                skillStats[activeTab].averageTime
              }}</span>
            </div>
            <div
              v-if="activeTab === 'Listening' || activeTab === 'Reading'"
              class="flex flex-col items-center bg-white shadow-lg rounded-lg p-2 m-2 w-[200px] shadow-md"
            >
              <p class="text-teal-500 text-lg mt-1 font-semibold">Accuracy</p>
              <span class="text-2xl font-semibold mt-1"
                >{{ skillStats[activeTab].averageAccuracy }}%
              </span>
            </div>
            <div
              class="flex flex-col items-center bg-white shadow-lg rounded-lg p-2 m-2 w-[200px] shadow-md"
            >
              <p class="text-teal-500 text-lg mt-1 font-semibold">
                Total Tests
              </p>
              <span class="text-2xl font-semibold mt-1"
                >{{ skillStats[activeTab].totalTest }}
              </span>
            </div>
          </div>
        </div>
      </div>
      <!-- <div class="mt-10 flex flex-col items-center">
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
      </div> -->
    </div>
  </div>
</template>
