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
        <div class="mt-4" v-if="testType === 'Listening'">
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
        <div
          v-else-if="testType === 'Writing'"
          class="flex flex-row mt-4 gap-2 border-b-4 border-gray-300"
        >
          <!-- Phần hiển thị đề bài -->
          <div class="flex flex-col w-1/2 bg-white rounded-lg">
            <div
              v-html="selectedRecording.passage"
              class="p-4 bg-gray-100 rounded-lg"
            ></div>
            <div>
              <div class="p-4 bg-gray-100 rounded-lg mt-4 shadow-md">
                <p class="text-teal-500 font-bold text-lg mb-4">Your Answer</p>

                <textarea
                  v-if="selectedRecording.id"
                  v-model="writingAnswers[selectedRecording.id]"
                  placeholder="Write your answer here..."
                  class="w-full p-4 resize-none bg-white overflow-y-auto rounded-lg"
                  style="max-height: 700px; min-height: 550px"
                ></textarea>
                <div
                  class="flex flex-row justify-between items-center p-4 bg-white rounded-lg"
                >
                  <p class="text-sm text-gray-500">
                    Word Count: <strong>{{ wordCount }}</strong>
                  </p>
                  <button
                    v-if="!isSubmitting && !isSubmitted"
                    class="bg-teal-500 text-white font-bold py-2 px-4 rounded-lg hover:bg-teal-600 cursor-pointer"
                    @click="submitWritingAnswers"
                  >
                    Submit Answer
                  </button>
                </div>
              </div>
            </div>
          </div>

          <div class="flex flex-col w-1/2 rounded-lg shadow-md bg-white">
            <div
              class="flex flex-row text-lg font-bold text-teal-500 text-center rounded-lg"
            >
              <button
                class="w-1/2 p-4 rounded-lg"
                :class="{
                  'opacity-50 cursor-not-allowed': !selectedRecording?.audioUrl,
                  'text-teal-500 bg-gray-100': !showImage,
                  'text-white bg-teal-500': showImage,
                  'cursor-pointer': selectedRecording?.audioUrl,
                }"
                @click="showImage = true"
                :disabled="!selectedRecording?.audioUrl"
              >
                Image
              </button>
              <button
                class="w-1/2 p-4 cursor-pointer rounded-lg"
                @click="showImage = false"
                :class="{
                  'text-teal-500 bg-gray-100': showImage,
                  'text-white bg-teal-500': !showImage,
                }"
              >
                AI Feedback
              </button>
            </div>

            <!-- Hiển thị hình ảnh -->
            <div
              v-if="showImage"
              class="overflow-y-auto rounded-lg bg-white mt-2"
              style="max-height: 740px; min-height: 700px"
            >
              <img
                v-if="selectedRecording?.audioUrl"
                :src="selectedRecording.audioUrl"
                class="p-4"
              />
              <p v-else class="text-center text-gray-500 p-4">
                No image available for this task.
              </p>
            </div>

            <!-- Hiển thị AI Feedback -->
            <div v-else>
              <div
                v-if="!isSubmitting && !isSubmitted"
                class="text-center p-6 mt-2 text-gray-500"
              >
                <i
                  >Your AI feedback will appear here after you submit your
                  answer.</i
                >
              </div>
              <div
                v-else-if="isSubmitting && !isSubmitted"
                class="text-center text-gray-500 p-6"
              >
                <div
                  class="flex flex-col items-center justify-center space-y-3 text-gray-500"
                >
                  <!-- Spinner Icon -->
                  <svg
                    class="animate-spin h-8 w-8 text-teal-500"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <circle
                      class="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      stroke-width="4"
                    ></circle>
                    <path
                      class="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"
                    ></path>
                  </svg>

                  <!-- Loading Text -->
                  <p class="italic">
                    Analyzing your writing... Please wait a moment.
                  </p>
                </div>
              </div>

              <div v-else class="text-center text-teal-600 p-4 space-y-8">
                <div v-if="aiFeedback">
                  <!-- Task 1 Feedback -->
                  <div
                    class="bg-white shadow-md rounded-xl border border-gray-100 p-6"
                  >
                    <h3
                      class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                    >
                      <svg
                        class="w-6 h-6 mr-2 text-teal-500"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2"
                        viewBox="0 0 24 24"
                      >
                        <path
                          d="M9 12l2 2l4 -4"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                        <circle
                          cx="12"
                          cy="12"
                          r="9"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                      </svg>
                      Task 1 Feedback
                    </h3>
                    <div
                      class="grid grid-cols-1 md:grid-cols-2 gap-4 text-left text-gray-700"
                    >
                      <p>
                        <strong>TR (Task Response):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.tr.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>CC (Coherence & Cohesion):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.cc.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>LR (Lexical Resource):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.lr.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong
                          >GRA (Grammatical Range & Accuracy):&nbsp;</strong
                        >
                        <span class="text-gray-600">{{
                          aiFeedback.task1.gra.toFixed(1)
                        }}</span>
                      </p>
                    </div>
                    <div class="mt-4 text-left text-gray-700">
                      <p>
                        <strong>Score:&nbsp;</strong>
                        <span class="text-gray-800 font-semibold">{{
                          aiFeedback.task1.score.toFixed(1)
                        }}</span>
                      </p>
                      <p class="mt-2">
                        <strong>Feedback:&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.feedback
                        }}</span>
                      </p>
                    </div>
                  </div>

                  <!-- Task 2 Feedback -->
                  <div
                    class="bg-white shadow-md rounded-xl border border-gray-100 p-6 mt-4"
                  >
                    <h3
                      class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                    >
                      <svg
                        class="w-6 h-6 mr-2 text-teal-500"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2"
                        viewBox="0 0 24 24"
                      >
                        <path
                          d="M9 12l2 2l4 -4"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                        <circle
                          cx="12"
                          cy="12"
                          r="9"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                      </svg>
                      Task 2 Feedback
                    </h3>
                    <div
                      class="grid grid-cols-1 md:grid-cols-2 gap-4 text-left text-gray-700"
                    >
                      <p>
                        <strong>TR (Task Response):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.tr.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>CC (Coherence & Cohesion):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.cc.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>LR (Lexical Resource):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.lr.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong
                          >GRA (Grammatical Range & Accuracy):&nbsp;</strong
                        >
                        <span class="text-gray-600">{{
                          aiFeedback.task2.gra.toFixed(1)
                        }}</span>
                      </p>
                    </div>
                    <div class="mt-4 text-left text-gray-700">
                      <p>
                        <strong>Score:&nbsp;</strong>
                        <span class="text-gray-800 font-semibold">{{
                          aiFeedback.task2.score.toFixed(1)
                        }}</span>
                      </p>
                      <p class="mt-2">
                        <strong>Feedback:&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.feedback
                        }}</span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div
          v-else-if="testType === 'Speaking'"
          class="flex flex-row mt-4 gap-4 border-b-4 border-gray-300"
        >
          <div class="flex flex-col w-1/2 bg-white rounded-lg">
            <!-- Phần hiển thị đề bài -->
            <div
              v-html="selectedRecording.passage"
              class="p-4 bg-gray-100 rounded-lg"
            ></div>

            <div
              class="bg-white p-6 rounded-xl shadow-md border border-gray-100 mt-6"
            >
              <h3 class="text-lg font-bold text-teal-500 mb-4">
                Your Speaking Recording
              </h3>

              <!-- Thời gian ghi âm -->
              <div class="text-center text-2xl font-mono text-gray-700 mb-4">
                {{ formatTime(recordingTime) }}
              </div>

              <!-- Nút điều khiển -->
              <div class="flex justify-center gap-4">
                <button
                  class="p-2 rounded-full transition text-white"
                  :class="
                    isRecording
                      ? 'bg-red-500 hover:bg-red-600'
                      : 'bg-teal-500 hover:bg-teal-600'
                  "
                  @click="toggleRecording"
                >
                  <component :is="isRecording ? Pause : Play" class="w-6 h-6" />
                </button>
              </div>

              <!-- Phát lại đoạn ghi âm -->
              <div v-if="recordedAudioUrls[selectedRecording?.id]" class="mt-4">
                <h4 class="text-teal-500 font-bold mb-2">Playback</h4>
                <audio
                  :src="recordedAudioUrls[selectedRecording?.id]"
                  controls
                  class="w-full mt-4"
                ></audio>
              </div>
              <div v-else class="text-center text-gray-500 p-4">
                <i>Your recording will appear here after you stop.</i>
              </div>
            </div>
          </div>

          <div class="flex flex-col w-1/2 rounded-lg shadow-md bg-white">
            <div
              class="flex flex-row text-lg font-bold text-teal-500 text-center rounded-lg"
            >
              <button
                class="w-1/2 p-4 rounded-lg cursor-pointer"
                :class="
                  activeTab === 'transcript'
                    ? 'text-white bg-teal-500'
                    : 'text-teal-500 bg-gray-100'
                "
                @click="activeTab = 'transcript'"
              >
                Transcript
              </button>
              <button
                class="w-1/2 p-4 cursor-pointer rounded-lg cursor-pointer"
                @click="activeTab = 'aiFeedback'"
                :class="
                  activeTab === 'aiFeedback'
                    ? 'text-white bg-teal-500'
                    : 'text-teal-500 bg-gray-100'
                "
              >
                AI Feedback
              </button>
            </div>

            <!-- Hiển thị hình ảnh -->
            <div v-if="activeTab === 'transcript'">
              <i
                v-if="!isSubmitted && !isSubmitting && !aiFeedback"
                class="text-center p-6 mt-2 text-gray-500"
              >
                Your transcript will appear here after you submit your
                answer.</i
              >
              <div
                v-else-if="aiFeedback"
                class="text-teal-600 flex flex-col items-stretch space-y-3 p-4"
              >
                <!-- Task 1 Transcript -->
                <div
                  class="bg-white shadow-md rounded-xl border border-gray-100 p-6"
                >
                  <h3
                    class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                  >
                    <svg
                      class="w-6 h-6 mr-2 text-teal-500"
                      fill="none"
                      stroke="currentColor"
                      stroke-width="2"
                      viewBox="0 0 24 24"
                    >
                      <path
                        d="M9 12l2 2l4 -4"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                      <circle
                        cx="12"
                        cy="12"
                        r="9"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                    Task 1 Transcript
                  </h3>
                  <div class="text-left text-gray-700">
                    <p v-html="aiFeedback.task1.transcript"></p>
                  </div>
                </div>

                <!-- Task 2 Transcript -->
                <div
                  class="bg-white shadow-md rounded-xl border border-gray-100 p-6"
                >
                  <h3
                    class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                  >
                    <svg
                      class="w-6 h-6 mr-2 text-teal-500"
                      fill="none"
                      stroke="currentColor"
                      stroke-width="2"
                      viewBox="0 0 24 24"
                    >
                      <path
                        d="M9 12l2 2l4 -4"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                      <circle
                        cx="12"
                        cy="12"
                        r="9"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                    Task 2 Transcript
                  </h3>
                  <div class="text-left text-gray-700">
                    <p v-html="aiFeedback.task2.transcript"></p>
                  </div>
                </div>

                <!-- Task 3 Transcript -->
                <div
                  class="bg-white shadow-md rounded-xl border border-gray-100 p-6"
                >
                  <h3
                    class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                  >
                    <svg
                      class="w-6 h-6 mr-2 text-teal-500"
                      fill="none"
                      stroke="currentColor"
                      stroke-width="2"
                      viewBox="0 0 24 24"
                    >
                      <path
                        d="M9 12l2 2l4 -4"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                      <circle
                        cx="12"
                        cy="12"
                        r="9"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                    Task 3 Transcript
                  </h3>
                  <div class="text-left text-gray-700">
                    <p v-html="aiFeedback.task3.transcript"></p>
                  </div>
                </div>
              </div>
            </div>

            <!-- Hiển thị AI Feedback -->
            <div v-else>
              <div
                v-if="!isSubmitted && !isSubmitting"
                class="text-center p-6 mt-2 text-gray-500"
              >
                <i
                  >Your AI feedback will appear here after you submit your
                  answer.</i
                >
              </div>

              <div
                v-else-if="isSubmitting && !isSubmitted"
                class="text-center text-gray-500 p-6"
              >
                <div
                  class="flex flex-col items-center justify-center space-y-3 text-gray-500"
                >
                  <!-- Spinner Icon -->
                  <svg
                    class="animate-spin h-8 w-8 text-teal-500"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <circle
                      class="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      stroke-width="4"
                    ></circle>
                    <path
                      class="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"
                    ></path>
                  </svg>

                  <!-- Loading Text -->
                  <p class="italic">
                    Analyzing your speaking... Please wait a moment.
                  </p>
                </div>
              </div>
              <div v-else>
                <div
                  v-if="aiFeedback"
                  class="text-center text-teal-600 p-4 space-y-4"
                >
                  <!-- Task 1 Feedback -->
                  <div
                    class="bg-white shadow-md rounded-xl border border-gray-100 p-6"
                  >
                    <h3
                      class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                    >
                      <svg
                        class="w-6 h-6 mr-2 text-teal-500"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2"
                        viewBox="0 0 24 24"
                      >
                        <path
                          d="M9 12l2 2l4 -4"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                        <circle
                          cx="12"
                          cy="12"
                          r="9"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                      </svg>
                      Task 1 Feedback
                    </h3>
                    <div
                      class="grid grid-cols-1 md:grid-cols-2 gap-4 text-left text-gray-700"
                    >
                      <p>
                        <strong>FC (Fluency & Coherence):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.fc.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>LR (Lexical Resource):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.lr.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong
                          >GRA (Grammatical Range & Accuracy):&nbsp;</strong
                        >
                        <span class="text-gray-600">{{
                          aiFeedback.task1.gra.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>PR (Pronunciation):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.pr.toFixed(1)
                        }}</span>
                      </p>
                    </div>
                    <div class="mt-4 text-left text-gray-700">
                      <p>
                        <strong>Score:&nbsp;</strong>
                        <span class="text-gray-800 font-semibold">{{
                          aiFeedback.task1.score.toFixed(1)
                        }}</span>
                      </p>
                      <p class="mt-2">
                        <strong>Feedback:&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task1.feedback
                        }}</span>
                      </p>
                    </div>
                  </div>

                  <!-- Task 2 Feedback -->
                  <div
                    class="bg-white shadow-md rounded-xl border border-gray-100 p-6"
                  >
                    <h3
                      class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                    >
                      <svg
                        class="w-6 h-6 mr-2 text-teal-500"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2"
                        viewBox="0 0 24 24"
                      >
                        <path
                          d="M9 12l2 2l4 -4"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                        <circle
                          cx="12"
                          cy="12"
                          r="9"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                      </svg>
                      Task 2 Feedback
                    </h3>
                    <div
                      class="grid grid-cols-1 md:grid-cols-2 gap-4 text-left text-gray-700"
                    >
                      <p>
                        <strong>FC (Fluency & Coherence):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.fc.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>LR (Lexical Resource):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.lr.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong
                          >GRA (Grammatical Range & Accuracy):&nbsp;</strong
                        >
                        <span class="text-gray-600">{{
                          aiFeedback.task2.gra.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>PR (Pronunciation):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.pr.toFixed(1)
                        }}</span>
                      </p>
                    </div>
                    <div class="mt-4 text-left text-gray-700">
                      <p>
                        <strong>Score:&nbsp;</strong>
                        <span class="text-gray-800 font-semibold">{{
                          aiFeedback.task2.score.toFixed(1)
                        }}</span>
                      </p>
                      <p class="mt-2">
                        <strong>Feedback:&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task2.feedback
                        }}</span>
                      </p>
                    </div>
                  </div>

                  <!-- Task 3 Feedback -->
                  <div
                    class="bg-white shadow-md rounded-xl border border-gray-100 p-6"
                  >
                    <h3
                      class="text-xl font-bold mb-4 flex items-center justify-center text-teal-500"
                    >
                      <svg
                        class="w-6 h-6 mr-2 text-teal-500"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2"
                        viewBox="0 0 24 24"
                      >
                        <path
                          d="M9 12l2 2l4 -4"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                        <circle
                          cx="12"
                          cy="12"
                          r="9"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                      </svg>
                      Task 3 Feedback
                    </h3>
                    <div
                      class="grid grid-cols-1 md:grid-cols-2 gap-4 text-left text-gray-700"
                    >
                      <p>
                        <strong>FC (Fluency & Coherence):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task3.fc.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>LR (Lexical Resource):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task3.lr.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong
                          >GRA (Grammatical Range & Accuracy):&nbsp;</strong
                        >
                        <span class="text-gray-600">{{
                          aiFeedback.task3.gra.toFixed(1)
                        }}</span>
                      </p>
                      <p>
                        <strong>PR (Pronunciation):&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task3.pr.toFixed(1)
                        }}</span>
                      </p>
                    </div>
                    <div class="mt-4 text-left text-gray-700">
                      <p>
                        <strong>Score:&nbsp;</strong>
                        <span class="text-gray-800 font-semibold">{{
                          aiFeedback.task3.score.toFixed(1)
                        }}</span>
                      </p>
                      <p class="mt-2">
                        <strong>Feedback:&nbsp;</strong>
                        <span class="text-gray-600">{{
                          aiFeedback.task3.feedback
                        }}</span>
                      </p>
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
          v-if="testType !== 'Writing' && testType !== 'Speaking'"
          class="bg-white text-teal-500 border-2 border-teal-500 font-bold p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-500 hover:text-white transition-colors duration-300 ease-in-out cursor-pointer mt-4"
          @click="submitTest"
        >
          Submit Test
        </button>
        <button
          v-if="testType === 'Speaking'"
          class="bg-white text-teal-500 border-2 border-teal-500 font-bold p-2 rounded-[33px] pl-4 pr-4 hover:bg-teal-500 hover:text-white transition-colors duration-300 ease-in-out cursor-pointer mt-4"
          @click="submitSpeakingAnswers"
        >
          Submit Test
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute, useRouter } from "vue-router";
import { ref, onMounted, watch, onUnmounted, computed, reactive } from "vue";
import axios from "axios";
import { jwtDecode } from "jwt-decode";
import { Play, Pause } from "lucide-vue-next";

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
const totalTime = ref(null); // 40 phút (đổi sang giây)
const timeRemaining = ref(null); // Thời gian còn lại (giây)
const userAnswers = ref({}); // Khởi tạo biến userAnswers để lưu trữ câu trả lời của người dùng
const testName = ref(""); // Khởi tạo biến testName để lưu trữ tên bài test
const test = ref({}); // Khởi tạo biến test để lưu trữ thông tin bài test
const writingAnswers = ref({}); // Khởi tạo biến writingAnswers để lưu trữ câu trả lời của người dùng cho phần viết

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
      if (testType.value === "Reading" || testType.value === "Listening") {
        submitTest();
      } else if (testType.value === "Writing") {
        submitWritingAnswers();
      }
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
      `${import.meta.env.VITE_BASE_URL}/api/listening/${testId}`
    ); // Gọi API để lấy thông tin bài test
    test.value = responseTest.data; // Lưu thông tin bài test vào biến test
    totalTime.value = responseTest.data.duration * 60; // Lưu thời gian bài test vào biến totalTime
    timeRemaining.value = totalTime.value;
    testName.value = responseTest.data.title; // Lưu tên bài test vào biến testName
    const response = await axios.get(
      `${import.meta.env.VITE_BASE_URL}/api/recordings/by-test/${testId}`
    ); // Gọi API để lấy recordings
    recordings.value = response.data;
    // Gọi API để lấy recording đầu tiên (mặc định)
    if (recordings.value.length > 0) {
      await fetchRecordingDetails(recordings.value[0].id);
      if (testType.value === "Listening" || testType.value === "Reading") {
        recordings.value.forEach((recording) => {
          // Gọi fetchParts cho từng recording
          fetchParts(recording.id);
        });
      }
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
// Mỗi khi `selectedRecording` thay đổi, đảm bảo tạo key trước
watch(selectedRecording, (newVal) => {
  if (newVal?.id && writingAnswers.value[newVal.id] === undefined) {
    writingAnswers.value[newVal.id] = "";
  }
});

const testType = ref(""); // Khởi tạo biến testType để lưu trữ loại bài kiểm tra
const numberOfQuestions = ref(0); // Khởi tạo biến numberOfQuestions để lưu trữ số lượng câu hỏi
const fetchRecordingDetails = async (recordingId) => {
  try {
    const response = await axios.get(
      `${import.meta.env.VITE_BASE_URL}/api/recordings/${recordingId}`
    ); // Gọi API để lấy thông tin bản ghi
    selectedRecording.value = response.data; // Cập nhật bản ghi đã chọn
    audioUrl.value = response.data.audioUrl; // Lưu URL âm thanh vào biến audioUrl
    testType.value = response.data.testType;
    numberOfQuestions.value = response.data.numberOfQuestions; // Lưu số lượng câu hỏi vào biến numberOfQuestions
  } catch (error) {
    console.error("Error fetching recording details:", error);
  }
};

const fetchParts = async (recordingId) => {
  if (testType.value === "Writing" || testType.value === "Speaking") return; // Không gọi API nếu là bài viết
  try {
    const response = await axios.get(
      `${import.meta.env.VITE_BASE_URL}/api/parts/by-recording/${recordingId}`
    ); // Gọi API để lấy parts
    partsByRecording.value[recordingId] = response.data;
    partsByRecording.value[recordingId].forEach((part) => {
      // Gọi fetchQuestions cho từng part
      fetchQuestions(part.id);
    });
  } catch (error) {
    console.error("Error fetching parts:", error);
  }
};

const fetchQuestions = async (partId) => {
  try {
    const response = await axios.get(
      `${import.meta.env.VITE_BASE_URL}/api/questions/by-part/${partId}`
    ); // Gọi API để lấy câu hỏi theo partId
    questionsByPart.value[partId] = response.data;

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
      `${import.meta.env.VITE_BASE_URL}/api/answers/by-question/${questionId}`
    ); // Gọi API để lấy câu hỏi theo partId
    answersByQuestion.value[questionId] = response.data;
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

  try {
    const totalQuestion = numberOfQuestions.value; // Đếm số lượng câu hỏi
    const correctAnswers = calculateCorrectAnswers(); // Tính số câu trả lời đúng
    const accuracy = (correctAnswers / totalQuestion) * 100; // Tính độ chính xác
    const score = correctAnswers * 10;

    const timeTaken = formatTime(totalTime.value - timeRemaining.value); // Tính thời gian đã sử dụng
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

    // Gửi dữ liệu đến API
    const response = await axios.post(
      `${import.meta.env.VITE_BASE_URL}/api/submit-test`,
      payload
    );

    alert("Test submitted successfully!");

    const testResultId = response.data.testResultId;

    // Chuyển hướng đến trang kết quả
    router.push({
      name: "TestResult",
      params: { id: testResultId },
    });

    // // Reset thời gian còn lại
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

    if (
      userAnswer &&
      correctAnswer &&
      userAnswer.trim().toLowerCase() === correctAnswer.trim().toLowerCase()
    ) {
      correctAnswers++;
    }
  }

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

// Đếm số từ trong câu trả lời
const wordCount = computed(() => {
  if (!selectedRecording.value || !selectedRecording.value.id) return 0;
  const answer = writingAnswers.value[selectedRecording.value.id] || "";
  return answer.trim() === "" ? 0 : answer.trim().split(/\s+/).length;
});

const showImage = ref(true); // Mặc định hiển thị hình ảnh
const isSubmitted = ref(false); // Trạng thái đã nộp bài
const aiFeedback = ref(""); // Phản hồi từ AI

const isSubmitting = ref(false); // Trạng thái đang gửi bài
const submitWritingAnswers = async () => {
  try {
    isSubmitting.value = true; // Đặt trạng thái đang gửi bài
    showImage.value = false; // Chuyển sang chế độ hiển thị phản hồi AI
    const task1Prompt = `You are an IELTS Writing Task 1 evaluator.
Below is a task and a student's answer. Evaluate it based on IELTS Writing band descriptors
(Task Achievement, Coherence & Cohesion, Lexical Resource, Grammatical Range & Accuracy).
Give overall feedback and an estimated band score (0-9).

Task:
${recordings.value[0].passage}

Student's Answer:
${writingAnswers.value[recordings.value[0].id]}

Image (if any):
${recordings.value[0].audioUrl || "No image"}
`;

    const task2Prompt = `You are an IELTS Writing Task 2 evaluator.
Evaluate the following essay based on IELTS Writing band descriptors
(Task Response, Coherence & Cohesion, Lexical Resource, Grammatical Range & Accuracy).
Give overall feedback and an estimated band score (0-9).

Task:
${recordings.value[1].passage}

Student's Answer:
${writingAnswers.value[recordings.value[1].id]}
Returns the result in JSON format as follows:

{
"task1": {
"score": 7.5,
"task response": 7.0,
"coherence & cohesion": 8.0,
"lexical resource": 8.0,
"grammatical range & accuracy": 8.0,
"feedback": "Overall feedback on task 1..."
},
"task2": {
"score": 8.0,
"task response": 8.0,
"coherence & cohesion": 8.0,
"lexical resource": 8.0,
"grammatical range & accuracy": 8.0,
"feedback": "Overall feedback on task 2..."
}
}
`;

    const payload = {
      task1Prompt,
      task2Prompt,
      userId,
      testId,
      timeTaken: formatTime(totalTime.value - timeRemaining.value), // Thời gian đã sử dụng
      testType: testType.value, // Loại bài kiểm tra
    };

    console.log("Payload:", payload);

    const response = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/writing/submit-to-feedback`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      }
    );

    if (!response.ok) {
      throw new Error("Gửi bài thất bại!");
    }

    const result = await response.json();
    console.log("Đánh giá từ AI:", result);

    aiFeedback.value = result;
    isSubmitting.value = false; // Đặt trạng thái không còn gửi bài

    isSubmitted.value = true;

    alert("Đã gửi bài viết thành công!");
  } catch (error) {
    console.error("Lỗi khi gửi bài:", error);
  }
};
const activeTab = ref("transcript"); // Mặc định hiển thị transcript
let mediaRecorder;
let audioChunks = [];

const isRecording = ref(false);
const recordingTime = ref(0);
let timer = null;

const recordedAudioUrls = ref({}); // Lưu URL của đoạn ghi âm theo recording ID

let recognition; // Biến lưu trữ đối tượng SpeechRecognition
let isSpeechRecognitionSupported = false; // Kiểm tra hỗ trợ SpeechRecognition
const transcripts = ref([]); // Lưu transcript trong thời gian thực

// Kiểm tra hỗ trợ Web Speech API
if ("SpeechRecognition" in window || "webkitSpeechRecognition" in window) {
  isSpeechRecognitionSupported = true;
  const SpeechRecognition =
    window.SpeechRecognition || window.webkitSpeechRecognition;
  recognition = new SpeechRecognition();
  recognition.continuous = true; // Cho phép nhận dạng liên tục
  recognition.interimResults = true; // Hiển thị kết quả tạm thời
  recognition.lang = "en-US"; // Ngôn ngữ nhận dạng
} else {
  console.warn("Web Speech API is not supported in this browser.");
}

function toggleRecording() {
  if (isRecording.value) {
    stopRecording().then(() => {
      isRecording.value = false; // Đặt lại trạng thái sau khi dừng
    });
  } else {
    startRecording().then(() => {
      isRecording.value = true; // Đặt trạng thái khi bắt đầu
    });
  }
}

const startRecording = async () => {
  const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
  mediaRecorder = new MediaRecorder(stream);
  audioChunks = [];

  mediaRecorder.ondataavailable = (e) => {
    audioChunks.push(e.data);
  };

  mediaRecorder.start();
  startTimer(); // Bắt đầu đếm thời gian
  isRecording.value = true;

  // Bắt đầu nhận dạng giọng nói
  if (isSpeechRecognitionSupported && recognition) {
    transcripts.value[selectedRecording.value.id] = ""; // Reset transcript cho recording hiện tại
    recognition.start();

    recognition.onresult = (event) => {
      let interimTranscript = "";
      for (let i = event.resultIndex; i < event.results.length; i++) {
        const result = event.results[i];
        if (result.isFinal) {
          // Tạo một bản sao của transcripts và cập nhật giá trị
          const updatedTranscripts = { ...transcripts.value };
          updatedTranscripts[selectedRecording.value.id] =
            (updatedTranscripts[selectedRecording.value.id] || "") +
            result[0].transcript +
            " ";
          transcripts.value = updatedTranscripts; // Gán lại toàn bộ đối tượng
        } else {
          interimTranscript += result[0].transcript; // Kết quả tạm thời
        }
      }
    };

    recognition.onerror = (event) => {
      console.error("SpeechRecognition error:", event.error);
    };

    recognition.onend = () => {
      console.log("SpeechRecognition stopped.");
    };
  }
};
const stopRecording = async () => {
  if (!mediaRecorder) {
    console.error("No mediaRecorder available to stop.");
    return;
  }

  return new Promise((resolve) => {
    mediaRecorder.onstop = async () => {
      const blob = new Blob(audioChunks, { type: "audio/webm" });

      try {
        const audioUrl = await URL.createObjectURL(blob); // Tạo URL cho blob âm thanh

        // Lưu lại vào bản ghi đang chọn
        if (selectedRecording.value?.id) {
          recordedAudioUrls.value[selectedRecording.value.id] = audioUrl;
        }

        resolve();
      } catch (err) {
        console.error("❌ Upload failed:", err);
      }
    };

    mediaRecorder.stop();
    stopTimer();
    isRecording.value = false;
    recordingTime.value = 0;

    // Dừng nhận dạng giọng nói
    if (isSpeechRecognitionSupported && recognition) {
      recognition.stop();
    }
    console.log(
      "Transcript cuối cùng:",
      transcripts.value[selectedRecording.value.id]
    ); // In ra transcript cuối cùng
  });
};

// Bắt đầu đếm thời gian
const startTimer = () => {
  timer = setInterval(() => {
    recordingTime.value++;
  }, 1000);
};

// Dừng đếm thời gian
const stopTimer = () => {
  clearInterval(timer);
  timer = null;
};

const submitSpeakingAnswers = async () => {
  try {
    console.log(
      "Gửi bài viết cho AI...:",
      transcripts.value[recordings.value[0].id]
    );
    console.log(
      "Gửi bài viết cho AI...:",
      transcripts.value[recordings.value[1].id]
    );
    console.log(
      "Gửi bài viết cho AI...:",
      transcripts.value[recordings.value[2].id]
    );
    isSubmitting.value = true; // Đặt trạng thái đang gửi bài
    activeTab.value = "aiFeedback"; // Chuyển sang chế độ hiển thị phản hồi AI
    const speakingPrompt = `
You are an IELTS Speaking evaluator. I'm a student preparing for the IELTS Speaking test.

You will evaluate 3 IELTS Speaking responses based on the official IELTS Speaking band descriptors:
1. Fluency and Coherence
2. Lexical Resource
3. Grammatical Range and Accuracy
4. Pronunciation

Each task includes a prompt and a student's audio response.

Please return your evaluation in the following JSON format, DON'T USE THE DIFFERENT FORMAT:

{
  "part1": {
    "score": <band score (0.0 - 9.0)>,
    "fluency and coherence": <score>,
    "lexical resource": <score>,
    "grammatical range and accuracy": <score>,
    "pronunciation": <score>,
    "feedback": "feedback for part 1",
    "transcript": "transcript of part 1"
  },
  "part2": {
    "score": <band score (0.0 - 9.0)>,
    "fluency and coherence": <score>,
    "lexical resource": <score>,
    "grammatical range and accuracy": <score>,
    "pronunciation": <score>,
    "feedback": "feedback for part 2",
    "transcript": "transcript of part 2"
  },
  "part3": {
    "score": <band score (0.0 - 9.0)>,
    "fluency and coherence": <score>,
    "lexical resource": <score>,
    "grammatical range and accuracy": <score>,
    "pronunciation": <score>,
    "feedback": "feedback for part 3",
    "transcript": "transcript of part 3"
  }
}

Speaking Task Part 1:
Prompt: ${recordings.value[0].passage}
Transcript recorded by student, listen it: ${
      transcripts.value[recordings.value[0].id]
    }

Speaking Task Part 2:
Prompt: ${recordings.value[1].passage}
Transcript recorded by student, listen it: ${
      transcripts.value[recordings.value[1].id]
    }

Speaking Task Part 3:
Prompt: ${recordings.value[2].passage}
Transcript recorded by student, listen it: ${
      transcripts.value[recordings.value[2].id]
    }
If the student doesn't answer, return same format and fill in the score with -1, feedback with "No answer", transcript with "No answer".`;

    const payload = {
      speakingPrompt,
      userId,
      testId,
      timeTaken: formatTime(totalTime.value - timeRemaining.value), // Thời gian đã sử dụng
      testType: testType.value, // Loại bài kiểm tra
    };

    console.log("Payload:", payload);

    const response = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/speaking/submit-to-feedback`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      }
    );

    if (!response.ok) {
      throw new Error("Gửi bài thất bại!");
    }

    const result = await response.json();
    console.log("Đánh giá từ AI:", result);

    aiFeedback.value = result;
    isSubmitting.value = false; // Đặt trạng thái không còn gửi bài

    isSubmitted.value = true;

    alert("Successfully submitted the writing!");
  } catch (error) {
    console.error("Lỗi khi gửi bài:", error);
  }
};
</script>

<style scoped>
.scrollable-content {
  max-height: 500px; /* Giới hạn chiều cao */
  overflow-y: auto; /* Hiển thị thanh cuộn khi nội dung vượt quá */
  padding-right: 8px; /* Tạo khoảng trống để tránh nội dung bị che bởi thanh cuộn */
}
</style>
