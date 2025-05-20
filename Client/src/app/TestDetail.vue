<script setup>
import { useRoute, useRouter } from "vue-router";
import { onMounted } from "vue";
import { ref } from "vue";
import Footer from "../components/Footer.vue";

const route = useRoute();
const router = useRouter();
const test = ref(route.state?.test || null);
const activeTab = ref("info");
const stateTraining = ref("full");
const results = ref(null);
const answers = ref(null);
const answersLeft = ref(null);
const answersMiddle = ref(null);
const answersRight = ref(null);

const fetchTest = async (id) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/listening/${id}`
    );
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    const data = await response.json();
    test.value = data;
    console.log("Test type:", test.value.type); // Log type để kiểm tra
    const resultResponse = await fetch(
      `${
        import.meta.env.VITE_BASE_URL
      }/api/results/test/${id}/user/${localStorage.getItem("userid")}`
    );
    results.value = await resultResponse.json();
    console.log("Results:", results.value);
  } catch (error) {
    console.error("Error fetching test:", error);
  }
};

const fetchAnswers = async (testId) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/answers/${testId}`
    );
    const data = await response.json();
    answers.value = data;
    console.log("Answers:", answers.value);
    // Chia mảng câu trả lời thành 2 cột
    const part = Math.ceil(answers.value.length / 3);
    answersLeft.value = answers.value.slice(0, part);
    answersMiddle.value = answers.value.slice(part, part * 2);
    answersRight.value = answers.value.slice(part * 2);
  } catch (error) {
    console.error("Error fetching answers:", error);
  }
};

onMounted(() => {
  if (!test.value) {
    const id = route.params.id;
    fetchTest(id);
  }
  document.title = "Test Detail";
});

const formatDate = (date) => {
  const options = { year: "numeric", month: "long", day: "numeric" };
  return new Date(date).toLocaleDateString(undefined, options);
};

const startTest = () => {
  // Logic to start the test
  const id = route.params.id;
  router.push(`/test/${id}/start`); // Redirect to the test start page
};

const formatDateToDDMMYYYY = (dateString) => {
  const date = new Date(dateString);
  const day = String(date.getDate()).padStart(2, "0"); // Lấy ngày, thêm số 0 nếu cần
  const month = String(date.getMonth() + 1).padStart(2, "0"); // Lấy tháng (0-based), thêm số 0 nếu cần
  const year = date.getFullYear(); // Lấy năm
  return `${day}-${month}-${year}`; // Trả về chuỗi định dạng DD-MM-YYYY
};
</script>
<template>
  <div class="bg-gray-100 min-h-screen flex flex-col">
    <div
      class="bg-white shadow-md rounded-lg p-6 min-w-[500px] w-[1000px] mx-auto mt-10"
    >
      <div v-if="test">
        <h1 class="text-3xl font-bold mb-4 text-teal-500">{{ test.title }}</h1>
        <div class="inline-flex gap-4 mb-4 p-2 bg-teal-400 rounded-[33px]">
          <button
            @click="activeTab = 'info'"
            :class="
              activeTab === 'info'
                ? 'bg-teal-500 text-white'
                : 'bg-teal-400 text-white'
            "
            class="bg-teal-400 text-white p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-500 cursor-pointer"
          >
            Test Information
          </button>
          <button
            @click="(activeTab = 'answer'), fetchAnswers(test.id)"
            :class="
              activeTab === 'answer'
                ? 'bg-teal-500 text-white'
                : 'bg-teal-400 text-white'
            "
            class="bg-teal-400 text-white p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-500 cursor-pointer"
          >
            Answer/Transcript
          </button>
        </div>
        <div
          v-if="activeTab === 'answer'"
          class="flex flex-row bg-white shadow-md rounded p-4 mt-2 border-2 border-teal-500"
        >
          <div class="w-1/3 p-2">
            <div
              v-for="answer in answersLeft"
              :key="answer.id"
              class="flex flex-row gap-2 mb-2"
            >
              <p
                class="bg-teal-100 text-teal-500 p-2 rounded-full w-10 h-10 flex items-center justify-center font-bold"
              >
                {{ answer.index }}
              </p>
              <p class="text-teal-500 font-bold p-2">
                {{ answer.answerText }}
              </p>
            </div>
          </div>
          <div class="w-1/3 p-2 pl-15">
            <div
              v-for="answer in answersMiddle"
              :key="answer.id"
              class="flex flex-row gap-2 mb-2"
            >
              <p
                class="bg-teal-100 text-teal-500 p-2 rounded-full w-10 h-10 flex items-center justify-center font-bold"
              >
                {{ answer.index }}
              </p>
              <p class="text-teal-500 font-bold p-2">
                {{ answer.answerText }}
              </p>
            </div>
          </div>
          <div class="w-1/3 p-2 pl-20">
            <div
              v-for="answer in answersRight"
              :key="answer.id"
              class="flex flex-row gap-2 mb-2"
            >
              <p
                class="bg-teal-100 text-teal-500 p-2 rounded-full w-10 h-10 flex items-center justify-center font-bold"
              >
                {{ answer.index }}
              </p>
              <p class="text-teal-500 font-bold p-2">
                {{ answer.answerText }}
              </p>
            </div>
          </div>
        </div>
        <div v-if="activeTab === 'info'">
          <div
            class="bg-white shadow-md rounded p-4 mt-2 border-2 border-teal-500"
          >
            <p><strong>Created At:</strong> {{ formatDate(test.createdAt) }}</p>
            <p><strong>Duration:</strong> {{ test.duration }} minutes</p>
            <p v-if="test.type != 'Writing'">
              <strong>Number of Questions:</strong> {{ test.numberOfQuestions }}
            </p>
            <p v-if="test.type == 'Writing'">
              <strong>Number of Tasks:</strong> 2
            </p>
          </div>
          <div v-if="Array.isArray(results) && results.length > 0">
            <h2 class="text-2xl font-bold mt-6 text-teal-500">
              Your Test Results:
            </h2>
            <div
              class="overflow-hidden border-2 border-teal-500 rounded-lg shadow-lg mt-4"
            >
              <table class="min-w-full bg-white">
                <thead
                  class="bg-teal-100 text-teal-500 font-bold border-b-2 border-teal-500"
                >
                  <tr>
                    <th class="px-4 py-2 text-center w-1/3">Test Date</th>
                    <th class="px-4 py-2 text-center w-1/3">Result</th>
                    <th class="px-4 py-2 text-center w-1/3">Time Taken</th>
                  </tr>
                </thead>
              </table>
              <div class="overflow-y-auto max-h-96">
                <table class="min-w-full bg-white">
                  <tbody class="text-gray-700">
                    <tr
                      v-for="result in results"
                      :key="result.id"
                      class="hover:bg-teal-50 transition-colors duration-200"
                    >
                      <td
                        class="px-4 py-2 text-center border-b border-gray-200 w-1/3 pl-6"
                      >
                        {{ formatDateToDDMMYYYY(result.testDate) }}
                      </td>
                      <td
                        class="px-4 py-2 text-center border-b border-gray-200 font-bold text-teal-500 w-1/3 pl-8"
                      >
                        {{ result.score }}
                      </td>
                      <td
                        class="px-4 py-2 text-center border-b border-gray-200 w-1/3 pl-12"
                      >
                        {{ result.timeTaken }}
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>

          <ul
            class="flex flex-row mt-4 text-gray-300 border-b-2 border-gray-300 font-bold"
          >
            <li
              @click="stateTraining = 'practice'"
              :class="
                stateTraining === 'practice'
                  ? 'text-teal-500 border-b-4 border-teal-500'
                  : 'text-gray-300'
              "
              class="p-2 hover:text-teal-500 hover:cursor-pointer hover:border-b-4 border-teal-500 pl-4 pr-4"
            >
              Practice
            </li>
            <li
              @click="stateTraining = 'full'"
              :class="
                stateTraining === 'full'
                  ? 'text-teal-500 border-b-4 border-teal-500'
                  : 'text-gray-300'
              "
              class="p-2 hover:text-teal-500 hover:cursor-pointer hover:border-b-4 border-teal-500 pl-4 pr-4"
            >
              Full Test
            </li>
          </ul>
          <div
            v-if="stateTraining === 'practice'"
            class="bg-white shadow-md rounded p-4 mt-4 border-2 border-teal-500"
          >
            <p class="flex p-2 bg-green-100 text-green-700 rounded-md mb-2">
              <img
                src="../assets/lightbub.png"
                alt=""
                class="w-5 h-5 mr-2 inline-block"
              />
              Pro tips: Practicing in sections and choosing the right time will
              help you focus on getting the questions right instead of feeling
              pressured to complete the test.
            </p>
            <p>Select the test you want to practice:</p>
            <div class="flex flex-col space-x-4 mt-2">
              <div class="flex flex-row items-center space-x-2 mb-2">
                <input
                  type="checkbox"
                  id="section1"
                  name="section"
                  value="section1"
                />
                <label for="section1" class="text-teal-500"
                  >Section 1 (10 questions)</label
                >
              </div>
              <div class="flex flex-row items-center space-x-2 mb-2">
                <input
                  type="checkbox"
                  id="section2"
                  name="section"
                  value="section2"
                />
                <label for="section2" class="text-teal-500"
                  >Section 2 (10 questions)</label
                >
              </div>
              <div class="flex flex-row items-center space-x-2 mb-2">
                <input
                  type="checkbox"
                  id="section3"
                  name="section"
                  value="section3"
                />
                <label for="section3" class="text-teal-500"
                  >Section 3 (10 questions)</label
                >
              </div>
              <div class="flex flex-row items-center space-x-2 mb-2">
                <input
                  type="checkbox"
                  id="section4"
                  name="section"
                  value="section4"
                />
                <label for="section4" class="text-teal-500"
                  >Section 4 (10 questions)</label
                >
              </div>
            </div>
          </div>
          <div
            v-if="stateTraining === 'full'"
            class="bg-white shadow-md rounded p-4 mt-4 border-2 border-teal-500"
          >
            <p
              class="p-2 bg-orange-100 text-orange-700 rounded-md mb-2 flex items-center"
            >
              <img
                src="../assets/warning.png"
                alt=""
                class="mr-2 inline-block"
              />
              Ready to start taking the full test? For best results, you should
              spend {{ test.duration }} minutes on this test.
            </p>
          </div>
          <button
            v-if="activeTab === 'info'"
            @click="startTest"
            class="bg-white text-teal-500 border-2 border-teal-500 font-bold p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-500 hover:text-white transition-colors duration-300 ease-in-out cursor-pointer mt-4"
          >
            START TEST
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.container {
  max-width: 800px;
}
</style>
