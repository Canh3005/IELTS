<template>
  <div class="bg-gray-100 min-h-screen flex flex-col items-center">
    <h1 class="text-3xl font-bold text-teal-500 mb-4 mt-10">
      {{ testName }}
    </h1>
    <div class="flex flex-row gap-4">
      <div class="bg-white shadow-md rounded-lg p-6 min-w-[500px] w-[1500px]">
        <ul class="flex flex-row space-x-4">
          <li
            v-for="recording in recordings"
            :key="recording.id"
            @click="
              fetchRecordingDetails(recording.id),
                setAudio(recording.audioUrl),
                fetchParts(recording.id)
            "
            :class="{
              'text-white bg-teal-500': selectedRecording?.id === recording.id,
            }"
            class="p-2 border-2 border-teal-500 rounded-[33px] pl-4 pr-4 hover:bg-teal-500 hover:text-white cursor-pointer"
          >
            {{ recording.title }}
          </li>
        </ul>
        <div class="mt-4">
          <audio
            ref="audioPlayer"
            controls
            class="w-full"
            v-if="audioUrl"
            key="audioUrl"
          >
            <source :src="audioUrl" type="audio/mpeg" />
            Your browser does not support the audio element.
          </audio>
        </div>
        <div
          v-if="testType === 'Listening'"
          class="flex flex-col mt-4 gap-2 border-b-4 border-gray-300 overflow-y-auto"
          style="max-height: 600px"
        >
          <div
            v-for="part in partsByRecording[selectedRecording?.id]"
            :key="part.id"
            class="flex flex-row mb-4"
          >
            <div
              v-if="part.type === 'BlankFilled'"
              class="flex flex-col bg-gray-100 rounded-lg p-4 w-3/5"
            >
              <div v-html="part.title"></div>
              <img :src="part.imageUrl" />
              <div v-html="part.text"></div>
              <div v-if="part.type === 'Choose'">
                <div
                  v-for="question in questionsByPart[part.id]"
                  :key="question.id"
                >
                  <div v-html="question.questionText"></div>
                </div>
              </div>
            </div>
            <div
              v-if="part.type === 'BlankFilled'"
              class="rounded-lg p-6 w-2/5"
            >
              <div
                v-for="(question, index) in questionsByPart[part.id]"
                :key="question.id"
                class="flex flex-row items-center mb-10 gap-2"
              >
                <!-- Lấy giá trị đầu tiên từ part.listOfQuestions và tăng dần -->
                <p
                  class="flex items-center justify-center rounded-full bg-teal-100 text-teal-500 w-10 h-10 font-bold"
                >
                  {{ parseListOfQuestions(part.listOfQuestions)[0] + index }}
                </p>

                <!-- Input để nhập đáp án, lưu theo question.id -->
                <input
                  type="text"
                  placeholder="Enter your answer"
                  class="h-full border-2 border-teal-500 rounded-md p-2 w-4/5"
                  v-model="userAnswers[question.id]"
                />
              </div>
            </div>
            <div
              v-if="part.type === 'Choose'"
              class="flex flex-col bg-gray-100 rounded-lg p-4 w-full"
            >
              <div v-html="part.title"></div>
              <div v-html="part.text"></div>
              <div>
                <div
                  v-for="question in questionsByPart[part.id]"
                  :key="question.id"
                >
                  <div v-html="question.questionText"></div>
                  <div
                    v-for="answer in answersByQuestion[question.id]"
                    :key="answer.id"
                    class="ml-2"
                  >
                    <input
                      type="radio"
                      :name="question.id"
                      :value="answer.answerText"
                      class="mr-2"
                      :id="answer.id"
                      v-model="userAnswers[question.id]"
                    />
                    <label v-html="answer.answerText"></label>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div
          v-else-if="testType === 'Reading'"
          class="flex flex-row mt-4 gap-2 border-b-4 border-gray-300"
        >
          <div
            v-html="selectedRecording.passage"
            class="mb-4 p-4 bg-gray-100 rounded-lg w-3/5 overflow-y-auto"
            style="max-height: 600px"
          ></div>
          <div
            class="flex flex-col w-2/5 overflow-y-auto"
            style="max-height: 600px"
          >
            <div
              v-for="part in partsByRecording[selectedRecording?.id]"
              :key="part.id"
              class="flex flex-col mb-4"
            >
              <div
                v-if="part.type === 'BlankFilled'"
                class="flex flex-col bg-gray-100 rounded-lg p-4"
              >
                <div v-html="part.title"></div>
                <div v-html="part.text"></div>
                <div v-if="part.type === 'Choose'">
                  <div
                    v-for="question in questionsByPart[part.id]"
                    :key="question.id"
                  >
                    <div v-html="question.questionText"></div>
                  </div>
                </div>
              </div>
              <div v-if="part.type === 'BlankFilled'" class="rounded-lg p-6">
                <div
                  v-for="(question, index) in questionsByPart[part.id]"
                  :key="question.id"
                  class="flex flex-row items-center mb-10 gap-2"
                >
                  <!-- Lấy giá trị đầu tiên từ part.listOfQuestions và tăng dần -->
                  <p
                    class="flex items-center justify-center rounded-full bg-teal-100 text-teal-500 w-10 h-10 font-bold"
                  >
                    {{ parseListOfQuestions(part.listOfQuestions)[0] + index }}
                  </p>

                  <!-- Input để nhập đáp án, lưu theo question.id -->
                  <input
                    type="text"
                    placeholder="Enter your answer"
                    class="h-full border-2 border-teal-500 rounded-md p-2 w-4/5"
                    v-model="userAnswers[question.id]"
                  />
                </div>
              </div>
              <div
                v-if="part.type === 'Choose'"
                class="flex flex-col bg-gray-100 rounded-lg p-4 w-full"
              >
                <div v-html="part.title"></div>
                <div v-html="part.text"></div>
                <div>
                  <div
                    v-for="question in questionsByPart[part.id]"
                    :key="question.id"
                  >
                    <div v-html="question.questionText"></div>
                    <div
                      v-for="answer in answersByQuestion[question.id]"
                      :key="answer.id"
                      class="ml-2"
                    >
                      <input
                        type="radio"
                        :name="question.id"
                        :value="answer.answerText"
                        class="mr-2"
                        :id="answer.id"
                        v-model="userAnswers[question.id]"
                      />
                      <label v-html="answer.answerText"></label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="flex flex-row justify-end mt-4">
          <button
            class="flex flex-row items-center cursor-pointer"
            :class="{
              'text-white': !hasNextRecording, // Khi có recording tiếp theo
              'text-teal-500': hasNextRecording, // Khi không có recording tiếp theo
            }"
            @click="goToNextRecording"
          >
            NEXT
            <img
              v-if="hasNextRecording"
              src="../assets/arrow-point-to-right.png"
              alt=""
              class="w-3 h-3"
            />
          </button>
        </div>
      </div>
      <div
        class="flex flex-col items-center bg-white shadow-md rounded-lg p-6 w-[200px]"
      >
        <div class="flex flex-col items-center rounded-lg p-4 bg-gray-100 mb-4">
          <p>Time Remaining:</p>
          <p>
            <strong>{{ formattedTime }}</strong>
          </p>
        </div>
        <button
          class="bg-white text-teal-500 border-2 border-teal-500 font-bold p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-500 hover:text-white transition-colors duration-300 ease-in-out cursor-pointer mt-4"
          @click="submitTest"
        >
          Submit Test
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute, useRouter } from "vue-router";
import { ref, onMounted, watch, onUnmounted, computed } from "vue";
import axios from "axios";
import { jwtDecode } from "jwt-decode";

const router = useRouter();
const route = useRoute();
const testId = route.params.id; // Lấy ID của bài test từ route params
const recordings = ref([]); // Khởi tạo biến recordings để lưu trữ dữ liệu
const partsByRecording = ref({}); // Khởi tạo biến parts để lưu trữ dữ liệu
const selectedRecording = ref(null); // Khởi tạo biến selectedRecording để lưu trữ bản ghi đã chọn
const audioUrl = ref(null); // Khởi tạo biến audioUrl để lưu trữ URL của bản ghi âm thanh
const audioPlayer = ref(null);
const currentTimes = ref({}); // Lưu thời gian đã nghe cho từng URL
const questionsByPart = ref({}); // Khởi tạo biến questions để lưu trữ câu hỏi
const answersByQuestion = ref({}); // Khởi tạo biến answers để lưu trữ câu trả lời
const totalTime = 40 * 60; // 40 phút (đổi sang giây)
const timeRemaining = ref(totalTime); // Thời gian còn lại (giây)
const userAnswers = ref({}); // Khởi tạo biến userAnswers để lưu trữ câu trả lời của người dùng
const testName = ref(""); // Khởi tạo biến testName để lưu trữ tên bài test

const parseListOfQuestions = (listOfQuestion) => {
  if (!listOfQuestion) return [];
  const [start, end] = listOfQuestion.split("-").map(Number);
  if (isNaN(start) || isNaN(end)) return [];
  return Array.from({ length: end - start + 1 }, (_, i) => start + i);
};

const goToNextRecording = async () => {
  if (!selectedRecording.value) return;

  // Tìm chỉ số của recording hiện tại
  const currentIndex = recordings.value.findIndex(
    (recording) => recording.id === selectedRecording.value.id
  );

  // Tìm recording tiếp theo
  const nextRecording = recordings.value[currentIndex + 1];

  if (nextRecording) {
    try {
      // Chuyển sang recording tiếp theo
      await fetchRecordingDetails(nextRecording.id);
      setAudio(nextRecording.audioUrl);
      await fetchParts(nextRecording.id);
    } catch (error) {
      console.error("Error switching to next recording:", error);
    }
  } else {
    console.log("No more recordings available.");
  }
};

const hasNextRecording = computed(() => {
  if (!selectedRecording.value) return false;
  const currentIndex = recordings.value.findIndex(
    (recording) => recording.id === selectedRecording.value.id
  );
  return currentIndex < recordings.value.length - 1;
});

const formattedTime = computed(() => {
  if (timeRemaining.value == null || timeRemaining.value < 0) {
    return "00:00";
  }
  const minutes = Math.floor(timeRemaining.value / 60);
  const seconds = timeRemaining.value % 60;
  return `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(
    2,
    "0"
  )}`;
});

let timerInterval;

onMounted(() => {
  const savedTime = localStorage.getItem("timeRemaining");
  if (savedTime) {
    timeRemaining.value = parseInt(savedTime, 10);
  }
  timerInterval = setInterval(() => {
    if (timeRemaining.value > 0) {
      timeRemaining.value -= 1; // Giảm thời gian còn lại mỗi giây
      localStorage.setItem("timeRemaining", timeRemaining.value);
    } else {
      timeRemaining.value = 0; // Đảm bảo không giảm xuống dưới 0
      clearInterval(timerInterval); // Dừng đếm ngược khi hết thời gian
      alert("Time is up! Submitting the test..."); // Thông báo khi hết thời gian
      submitTest(); // Gọi hàm submitTest khi hết thời gian
    }
  }, 1000);
});

onUnmounted(() => {
  if (timerInterval) {
    clearInterval(timerInterval); // Dừng đếm ngược khi component bị hủy
  }
  localStorage.removeItem("timeRemaining"); // Xóa thời gian còn lại khỏi localStorage
});

onMounted(async () => {
  document.title = "Start Test"; // Đặt tiêu đề trang
  // Bắt đầu đếm ngược
  try {
    // Gọi API để lấy tên test
    const responseTest = await axios.get(
      `http://localhost:5004/api/listening/${testId}`
    ); // Gọi API để lấy thông tin bài test
    testName.value = responseTest.data.title; // Lưu tên bài test vào biến testName
    console.log("Test Name:", testName.value); // In ra tên bài test
    const response = await axios.get(
      `http://localhost:5004/api/recordings/by-test/${testId}`
    ); // Gọi API để lấy recordings
    recordings.value = response.data;
    // Gọi API để lấy recording đầu tiên (mặc định)
    if (recordings.value.length > 0) {
      await fetchRecordingDetails(recordings.value[0].id);
      recordings.value.forEach((recording) => {
        // Gọi fetchParts cho từng recording
        fetchParts(recording.id);
      });
    }
  } catch (error) {
    console.error("Error fetching recordings:", error);
  }
});

const setAudio = (url) => {
  // Lưu thời gian của recording hiện tại trước khi chuyển
  if (audioUrl.value && audioPlayer.value) {
    currentTimes.value[audioUrl.value] = audioPlayer.value.currentTime;
  }

  // Cập nhật audioUrl mới
  audioUrl.value = url;
  console.log("Audio URL:", audioUrl.value); // In ra URL âm thanh
};

watch(audioUrl, (newUrl) => {
  if (audioPlayer.value) {
    audioPlayer.value.load(); // force reload audio
    audioPlayer.value.onloadedmetadata = () => {
      // Set lại thời gian nếu đã từng nghe
      const time = currentTimes.value[newUrl] || 0;
      audioPlayer.value.currentTime = time;
    };
  }
});

const testType = ref(""); // Khởi tạo biến testType để lưu trữ loại bài kiểm tra
const numberOfQuestions = ref(0); // Khởi tạo biến numberOfQuestions để lưu trữ số lượng câu hỏi
const fetchRecordingDetails = async (recordingId) => {
  try {
    const response = await axios.get(
      `http://localhost:5004/api/recordings/${recordingId}`
    ); // Gọi API để lấy thông tin bản ghi
    selectedRecording.value = response.data; // Cập nhật bản ghi đã chọn
    console.log("Selected Recording:", response.data); // In ra thông tin bản ghi đã chọn
    audioUrl.value = response.data.audioUrl; // Lưu URL âm thanh vào biến audioUrl
    testType.value = response.data.testType;
    console.log("Test Type:", testType.value); // In ra loại bài kiểm tra
    numberOfQuestions.value = response.data.numberOfQuestions; // Lưu số lượng câu hỏi vào biến numberOfQuestions
    console.log("Recording Details:", audioUrl.value); // In ra thông tin bản ghi
  } catch (error) {
    console.error("Error fetching recording details:", error);
  }
};

const fetchParts = async (recordingId) => {
  try {
    const response = await axios.get(
      `http://localhost:5004/api/parts/by-recording/${recordingId}`
    ); // Gọi API để lấy parts
    partsByRecording.value[recordingId] = response.data;
    partsByRecording.value[recordingId].forEach((part) => {
      // Gọi fetchQuestions cho từng part
      fetchQuestions(part.id);
    });
    console.log("Parts:", partsByRecording.value[recordingId]); // In ra dữ liệu parts
  } catch (error) {
    console.error("Error fetching parts:", error);
  }
};

const fetchQuestions = async (partId) => {
  try {
    const response = await axios.get(
      `http://localhost:5004/api/questions/by-part/${partId}`
    ); // Gọi API để lấy câu hỏi theo partId
    questionsByPart.value[partId] = response.data;
    console.log(`Questions by part ${partId}:`, response.data); // In ra dữ liệu câu hỏi

    // Theo dõi câu hỏi của partId
    // Gọi fetchAnswers trực tiếp cho từng câu hỏi

    questionsByPart.value[partId].forEach((question) => {
      fetchAnswers(question.id);
    });
  } catch (error) {
    console.error("Error fetching questions:", error);
  }
};

const fetchAnswers = async (questionId) => {
  try {
    const response = await axios.get(
      `http://localhost:5004/api/answers/by-question/${questionId}`
    ); // Gọi API để lấy câu hỏi theo partId
    answersByQuestion.value[questionId] = response.data;
    console.log(`Answers by question ${questionId}:`, response.data); // In ra dữ liệu câu hỏi
  } catch (error) {
    console.error("Error fetching questions:", error);
  }
};

const submitTest = async () => {
  // Hiển thị thông báo xác nhận
  const confirmSubmit = window.confirm(
    "Are you sure you want to submit the test? Once submitted, you cannot make changes."
  );

  if (!confirmSubmit) {
    // Nếu người dùng nhấn Cancel, thoát khỏi hàm
    return;
  }
  // console.log("Submitting test with answers:", userAnswers.value); // In ra câu trả lời của người dùng
  try {
    const totalQuestion = numberOfQuestions.value; // Đếm số lượng câu hỏi
    const correctAnswers = calculateCorrectAnswers(); // Tính số câu trả lời đúng
    const accuracy = (correctAnswers / totalQuestion) * 100; // Tính độ chính xác
    console.log("Total Questions:", numberOfQuestions.value); // In ra tổng số câu hỏi
    const score = correctAnswers * 10;

    const timeTaken = formatTime(totalTime - timeRemaining.value); // Tính thời gian đã sử dụng
    const answers = Object.keys(answersByQuestion.value).map((questionId) => {
      const userAnswer = userAnswers.value[questionId]
        ? userAnswers.value[questionId].includes(".")
          ? userAnswers.value[questionId].split(".")[0].trim() // Lấy ký tự đầu tiên trước dấu "."
          : userAnswers.value[questionId].trim() // Giữ nguyên nếu không có dấu "."
        : "No answer";
      const correctAnswer = answersByQuestion.value[questionId]
        ?.find((answer) => answer.isCorrect)
        ?.answerText.includes(".")
        ? answersByQuestion.value[questionId]
            ?.find((answer) => answer.isCorrect)
            ?.answerText.split(".")[0]
            .trim() // Lấy ký tự đầu tiên trước dấu "."
        : answersByQuestion.value[questionId]
            ?.find((answer) => answer.isCorrect)
            ?.answerText.trim(); // Giữ nguyên nếu không có dấu "."
      return {
        questionId: parseInt(questionId),
        userAnswer: userAnswer,
        correctAnswer: correctAnswer || "No correct answer",
      };
    });

    // Tạo payload để gửi đến backend
    const payload = {
      userId: userId, // ID người dùng (cần lấy từ session hoặc auth)
      testId: testId,
      accuracy: parseFloat(accuracy.toFixed(2)), // Làm tròn 2 chữ số thập phân
      score: parseFloat(score.toFixed(2)), // Làm tròn 2 chữ số thập phân
      testType: testType.value, // Loại bài kiểm tra
      timeTaken: timeTaken, // Thời gian đã sử dụng
      answers: answers, // Câu trả lời của người dùng
    };

    console.log("Payload:", payload);

    // Gửi dữ liệu đến API
    const response = await axios.post(
      "http://localhost:5004/api/submit-test",
      payload
    );

    console.log(response.data.message);
    alert("Test submitted successfully!");

    const testResultId = response.data.testResultId;

    // Chuyển hướng đến trang kết quả
    router.push({
      name: "TestResult",
      params: { id: testResultId },
    });

    // Reset thời gian còn lại
    timeRemaining.value = totalTime; // Đặt lại thời gian còn lại về 40 phút
    localStorage.removeItem("timeRemaining"); // Xóa thời gian còn lại khỏi localStorage
  } catch (error) {
    console.error("Error submitting test:", error);
    alert("Failed to submit test.");
  }
};

// Tính số câu trả lời đúng
const calculateCorrectAnswers = () => {
  let correctAnswers = 0;
  for (const questionId in userAnswers.value) {
    const userAnswer = userAnswers.value[questionId];
    const correctAnswer = answersByQuestion.value[questionId]?.find(
      (answer) => answer.isCorrect
    )?.answerText;

    console.log(
      `Question ID: ${questionId}, User Answer: ${userAnswer}, Correct Answer: ${correctAnswer}`
    ); // In ra thông tin câu hỏi và câu trả lời
    if (
      userAnswer &&
      correctAnswer &&
      userAnswer.trim().toLowerCase() === correctAnswer.trim().toLowerCase()
    ) {
      correctAnswers++;
    }
  }
  console.log("Correct Answers:", correctAnswers); // In ra số câu trả lời đúng

  return correctAnswers;
};

// Hàm format thời gian (giây -> mm:ss)
const formatTime = (seconds) => {
  const minutes = Math.floor(seconds / 60);
  const remainingSeconds = seconds % 60;
  return `${String(minutes).padStart(2, "0")}:${String(
    remainingSeconds
  ).padStart(2, "0")}`;
};

const getUserIdFromToken = () => {
  const token = localStorage.getItem("token"); // Lấy token từ localStorage
  if (!token) return null;

  const decoded = jwtDecode(token); // Giải mã token
  return decoded.nameid; // `nameid` là ClaimTypes.NameIdentifier
};

const userId = getUserIdFromToken();
console.log("User ID:", userId); // In ra ID người dùng từ token
</script>

<style scoped>
.scrollable-content {
  max-height: 500px; /* Giới hạn chiều cao */
  overflow-y: auto; /* Hiển thị thanh cuộn khi nội dung vượt quá */
  padding-right: 8px; /* Tạo khoảng trống để tránh nội dung bị che bởi thanh cuộn */
}
</style>
