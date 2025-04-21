<script setup>
import { useRoute, useRouter } from "vue-router";
import { onMounted } from "vue";
import { ref } from "vue";
import axios from "axios";

const route = useRoute();
const testResultId = route.params.id;
console.log(testResultId);
const result = ref(null);
const test = ref(null);
const answers = ref(null);
const countTrue = ref(0);
const countFalse = ref(0);
const countSkip = ref(0);
const answersLeft = ref(0);
const answersRight = ref(0);
const totalQuestions = ref(0);

onMounted(async () => {
  document.title = "Test Result";
  try {
    const response = await axios.get(
      `http://localhost:5004/api/results/${testResultId}`
    );
    const testId = response.data.testId;
    const testResponse = await axios.get(
      `http://localhost:5004/api/listening/${testId}`
    );
    test.value = testResponse.data;
    console.log(test.value);
    result.value = response.data;
    console.log(result.value);
    const answerResponse = await axios.get(
      `http://localhost:5004/api/results/${testResultId}/answers`
    );
    answers.value = answerResponse.data;
    console.log(answers.value);

    // Chia mảng câu trả lời thành 2 cột
    const half = Math.ceil(answers.value.length / 2);
    answersLeft.value = answers.value.slice(0, half);
    answersRight.value = answers.value.slice(half);

    // Tính tổng số câu, số câu đúng, sai và bỏ qua
    if (Array.isArray(answers.value)) {
      totalQuestions.value = answers.value.length;
    } else {
      console.error("Answers is not an array:", answers.value);
    }
    answers.value.forEach((answer) => {
      if (answer.uAnswer === "No answer") {
        countSkip.value++;
      } else if (
        answer.correctAnswer.trim().toLowerCase() ===
        answer.uAnswer.trim().toLowerCase()
      ) {
        countTrue.value++;
      } else {
        countFalse.value++;
      }
    });
  } catch (error) {
    console.error("Error fetching test result:", error);
  }
});
</script>
<template>
  <div class="bg-gray-100 min-h-screen flex flex-col">
    <div
      class="bg-white shadow-md rounded-lg p-6 min-w-[500px] w-[1000px] mx-auto mt-10"
    >
      <div>
        <h1 class="text-3xl font-bold mb-4 text-teal-500" v-if="test">
          Test Result: {{ test.title }}
        </h1>
        <div class="flex flex-row gap-2">
          <button
            class="text-white bg-teal-500 p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-600 cursor-pointer"
          >
            See answer
          </button>
          <button
            class="text-teal-500 p-2 rounded-[33px] pl-4 pr-4 bg-white border-2 hover:bg-teal-600 hover:text-white cursor-pointer"
            @click="
              $router.push({ name: 'TestDetail', params: { id: test.id } })
            "
          >
            Return to exam page
          </button>
        </div>
        <div class="flex flex-row gap-4 mt-4 justify-between">
          <div class="bg-gray-200 shadow-md rounded p-3 mt-2 w-[300px]">
            <div class="flex flex-row gap-2 mb-2 p-1">
              <img src="/TotalTests.png" alt="" class="w-7 h-7" />
              <p>
                Test results:
                <strong>{{ countTrue }}/{{ totalQuestions }}</strong>
              </p>
            </div>
            <div class="flex flex-row gap-2 mb-2 p-1">
              <img src="/Accuracy.png" alt="" class="w-7 h-7" />
              <p v-if="result">
                Accuracy: <strong>{{ result.accuracy }} %</strong>
              </p>
            </div>
            <div class="flex flex-row gap-2 mb-2 p-1">
              <img src="/AverageTime.png" alt="" class="w-7 h-7" />
              <p v-if="result">
                Time taken: <strong>{{ result.timeTaken }}</strong>
              </p>
            </div>
          </div>
          <div
            class="flex flex-col bg-white shadow-md rounded p-2 mt-2 w-[150px] text-center border-2 border-teal-500 items-center"
          >
            <img src="/correct.png" alt="" class="w-5 h-5 mt-4" />
            <p class="text-green-500 font-bold">Correct answer</p>
            <p class="font-bold">{{ countTrue }}</p>
            <p>question</p>
          </div>
          <div
            class="flex flex-col bg-white shadow-md rounded p-2 mt-2 w-[150px] text-center border-2 border-teal-500 items-center"
          >
            <img src="/incorrect.png" alt="" class="w-6 h-6 mt-4" />
            <p class="text-red-500 font-bold">Wrong answer</p>
            <p class="font-bold">{{ countFalse }}</p>
            <p>question</p>
          </div>
          <div
            class="flex flex-col bg-white shadow-md rounded p-2 mt-2 w-[150px] text-center border-2 border-teal-500 items-center"
          >
            <img src="/skip.png" alt="" class="w-5.5 h-5.5 mt-4" />
            <p class="text-gray-500 font-bold">Skip</p>
            <p class="font-bold">{{ countSkip }}</p>
            <p>question</p>
          </div>
          <div
            class="flex flex-col bg-white shadow-md rounded p-2 mt-2 w-[150px] text-center border-2 border-teal-500 items-center"
          >
            <img src="/point.png" alt="" class="w-6 h-6 mt-4" />
            <p class="text-blue-500 font-bold">Point</p>
            <p class="font-bold text-4xl mt-2" v-if="result">
              {{ result.score }}
            </p>
          </div>
        </div>
        <h2 class="text-2xl font-bold mt-6 text-teal-500">Details</h2>
        <div
          class="border-2 border-gray-200 mt-4 p-4 flex flex-row rounded-lg overflow-y-auto"
          style="max-height: 600px"
        >
          <div class="flex flex-col w-1/2 ml-10">
            <div
              class="flex flex-row gap-2 mb-4 items-center border-b-2 border-b-gray-300 pb-2"
              v-for="(answer, index) in answersLeft"
              :key="answer.id"
            >
              <p
                class="flex items-center justify-center rounded-full w-10 h-10 font-bold"
                :class="{
                  'bg-green-100 text-green-500 ':
                    answer.uAnswer.trim().toLowerCase() ===
                    answer.correctAnswer.trim().toLowerCase(),
                  'bg-red-100 text-red-500 ':
                    answer.uAnswer.trim().toLowerCase() !==
                      answer.correctAnswer.trim().toLowerCase() &&
                    answer.uAnswer !== 'No answer',
                  'bg-gray-100 text-gray-500': answer.uAnswer === 'No answer',
                }"
              >
                {{ 1 + index }}
              </p>
              <p class="text-center font-bold text-teal-500">
                {{ answer.correctAnswer }}:
              </p>
              <p
                :class="{
                  'text-red-500': answer.uAnswer === 'No answer', // Bôi đỏ nếu không có câu trả lời
                  'line-through':
                    answer.uAnswer.trim().toLowerCase() !==
                      answer.correctAnswer.trim().toLowerCase() &&
                    answer.uAnswer !== 'No answer',
                }"
              >
                <i>{{ answer.uAnswer }}</i>
              </p>
            </div>
          </div>
          <div class="flex flex-col w-1/2 ml-10">
            <div
              class="flex flex-row gap-2 mb-4 items-center border-b-2 border-b-gray-300 pb-2"
              v-for="(answer, index) in answersRight"
              :key="answer.id"
            >
              <p
                class="flex items-center justify-center rounded-full w-10 h-10 font-bold"
                :class="{
                  'bg-green-100 text-green-500 ':
                    answer.uAnswer.trim().toLowerCase() ===
                    answer.correctAnswer.trim().toLowerCase(),
                  'bg-red-100 text-red-500 ':
                    answer.uAnswer.trim().toLowerCase() !==
                      answer.correctAnswer.trim().toLowerCase() &&
                    answer.uAnswer !== 'No answer',
                  'bg-gray-100 text-gray-500': answer.uAnswer === 'No answer',
                }"
              >
                {{ 21 + index }}
              </p>
              <p class="text-center font-bold text-teal-500">
                {{ answer.correctAnswer }}:
              </p>
              <p
                :class="{
                  'text-red-500': answer.uAnswer === 'No answer', // Bôi đỏ nếu không có câu trả lời
                  'line-through':
                    answer.uAnswer.trim().toLowerCase() !==
                      answer.correctAnswer.trim().toLowerCase() &&
                    answer.uAnswer !== 'No answer',
                }"
              >
                <i>{{ answer.uAnswer }}</i>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
