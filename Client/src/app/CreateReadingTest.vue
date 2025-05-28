<script setup>
import "prosemirror-view/style/prosemirror.css";
import { ref } from "vue";
import { useEditor, EditorContent } from "@tiptap/vue-3";
import StarterKit from "@tiptap/starter-kit";
import Placeholder from "@tiptap/extension-placeholder";
import { useRouter } from "vue-router";
const router = useRouter();

const testTitle = ref("");

// Tạo 3 passage, mỗi passage có editor riêng
const passages = ref([
  {
    editor: useEditor({
      extensions: [
        StarterKit,
        Placeholder.configure({
          placeholder: "Nhập nội dung Passage 1...",
        }),
      ],
      content: "",
    }),
  },
  {
    editor: useEditor({
      extensions: [
        StarterKit,
        Placeholder.configure({
          placeholder: "Nhập nội dung Passage 2...",
        }),
      ],
      content: "",
    }),
  },
  {
    editor: useEditor({
      extensions: [
        StarterKit,
        Placeholder.configure({
          placeholder: "Nhập nội dung Passage 3...",
        }),
      ],
      content: "",
    }),
  },
]);

const sections = ref([
  {
    passageIndex: 0,
    title: "",
    type: "choose", // Loại câu hỏi, có thể là "multi" hoặc "blank"
    listOfQuestions: "",
    textBlankQuestion: "",
    blankAnswers: [
      {
        blankAnswer: "",
      },
    ],
    questions: [
      {
        type: "choose",
        question: "",
        options: ["", "", "", ""],
        answer: "",
      },
    ],
  },
]);

const addSection = () => {
  sections.value.push({
    passageIndex: 0,
    title: "",
    type: "choose", // Loại câu hỏi, có thể là "choose" hoặc "blank"
    listOfQuestions: "",
    textBlankQuestion: "",
    blankAnswers: [
      {
        blankAnswer: "",
      },
    ], // Dùng cho loại câu hỏi "blank"
    // Mỗi section có thể có nhiều câu hỏi
    questions: [
      {
        question: "",
        options: ["", "", "", ""],
        answer: "",
      },
    ],
  });
};

const addBlankAnswer = (sectionIdx) => {
  sections.value[sectionIdx].blankAnswers.push({
    blankAnswer: "",
  });
};

const addQuestion = (sectionIdx) => {
  sections.value[sectionIdx].questions.push({
    type: "choose",
    question: "",
    options: ["", "", "", ""],
    answer: "",
  });
};

const removeBlankAnswer = (sectionIdx, answerIdx) => {
  sections.value[sectionIdx].blankAnswers.splice(answerIdx, 1);
};

const removeSection = (sectionIdx) => {
  sections.value.splice(sectionIdx, 1);
};

const removeQuestion = (sectionIdx, questionIdx) => {
  sections.value[sectionIdx].questions.splice(questionIdx, 1);
};

const handleSubmit = async () => {
  // Lấy nội dung HTML của từng passage

  const passageContents = passages.value.map((p) => p.editor?.getHTML() || "");
  console.log({
    title: testTitle.value,
    passages: passageContents,
    sections: sections.value,
  });
  const payload = {
    title: testTitle.value,
    passages: passageContents,
    sections: sections.value,
  };
  try {
    const res = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/reading/create`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      }
    );
    if (res.ok) {
      alert("Tạo đề Reading thành công!");
      router.push("/reading"); // Chuyển hướng đến danh sách đề Reading
    } else {
      console.error("Error creating reading test:", res);
      alert("Có lỗi xảy ra khi tạo đề Reading!");
    }
  } catch (err) {
    console.error("Error creating reading test:", err);
    alert("Không thể kết nối server!");
  }
};
</script>

<template>
  <div class="bg-gray-100 min-h-screen flex flex-col items-center pt-10">
    <h1 class="text-3xl font-bold text-teal-500 mb-6">
      Create Reading IELTS Test
    </h1>
    <form
      class="bg-white shadow-lg rounded-lg p-8 w-[900px] flex flex-col gap-6"
      @submit.prevent="handleSubmit"
    >
      <label class="font-semibold text-2xl text-gray-700">Test Title</label>
      <input
        v-model="testTitle"
        class="border p-2 rounded"
        placeholder="Test Title"
      />

      <div v-for="(passage, pIdx) in passages" :key="pIdx" class="mb-2">
        <label class="font-semibold text-xl text-gray-700"
          >Passage {{ pIdx + 1 }}</label
        >

        <EditorContent
          :editor="passage.editor"
          class="border p-2 rounded w-full min-h-[120px] mt-4 bg-white"
        />
      </div>

      <div
        v-for="(section, sIdx) in sections"
        :key="sIdx"
        class="border rounded p-4 mb-4"
      >
        <div class="flex gap-4 flex-col mb-2">
          <div class="flex flex-row justify-between">
            <p class="font-semibold text-lg text-gray-700">
              Section {{ sIdx + 1 }}
            </p>
            <button
              type="button"
              class="bg-red-100 text-red-700 rounded p-2 ml-2"
              @click="removeSection(sIdx)"
            >
              Delete
            </button>
          </div>
          <div class="flex flex-row">
            <div class="flex flex-row items-center">
              <label class="font-semibold text-lg text-gray-700">From:</label>
              <select
                v-model="section.passageIndex"
                class="border rounded p-1 ml-2"
              >
                <option v-for="(p, pIdx) in passages" :key="pIdx" :value="pIdx">
                  Passage {{ pIdx + 1 }}
                </option>
              </select>
            </div>
            <div class="flex flex-row items-center ml-auto self-center">
              <label class="font-semibold">Type: </label>
              <select v-model="section.type" class="border rounded p-1 ml-2">
                <option value="choose">Multi-choice</option>
                <option value="blank">Blank-filled</option>
              </select>
            </div>
            <div class="ml-auto self-end">
              <label class="font-semibold text-lg text-gray-700"
                >List of Questions:</label
              >
              <input
                type="text"
                v-model="section.listOfQuestions"
                class="border rounded p-1 w-[100px] ml-2 self-end"
              />
            </div>
          </div>
          <label
            class="font-semibold text-lg text-gray-700"
            v-if="section.type !== 'choose'"
            >Section Title</label
          >
        </div>
        <input
          v-if="section.type !== 'choose'"
          v-model="section.title"
          class="border p-2 rounded mb-2 w-full"
          placeholder="Section Title"
        />

        <div v-if="section.type === 'choose'" class="mb-4">
          <p class="font-semibold text-lg text-gray-700 mb-2">Text</p>
          <textarea
            v-model="section.textBlankQuestion"
            class="border rounded p-2 w-full mb-2 min-h-[100px]"
            placeholder="Text for multi-choice questions"
          ></textarea>
          <div
            v-for="(q, qIdx) in section.questions"
            :key="qIdx"
            class="mb-4 border-b pb-4"
          >
            <div class="flex gap-4 mb-2 flex-col mt-2">
              <div class="flex flex-row items-center">
                <span class="font-semibold">Question {{ qIdx + 1 }}</span>
                <button
                  type="button"
                  class="bg-red-100 text-red-700 rounded p-2 ml-2 ml-auto self-end"
                  @click="removeQuestion(sIdx, qIdx)"
                >
                  Delete
                </button>
              </div>
            </div>
            <textarea
              v-model="q.question"
              class="border rounded p-2 w-full mb-2"
              placeholder="Question text"
            ></textarea>

            <div>
              <div
                v-for="(opt, oIdx) in q.options"
                :key="oIdx"
                class="flex items-center gap-2 mb-1"
              >
                <input
                  type="radio"
                  :name="`answer-${sIdx}-${qIdx}`"
                  :value="oIdx"
                  v-model="q.answer"
                />
                <input
                  v-model="q.options[oIdx]"
                  class="border rounded p-1 w-3/4"
                  :placeholder="`Option ${String.fromCharCode(65 + oIdx)}`"
                />
              </div>
            </div>
          </div>
          <button
            type="button"
            class="bg-teal-100 text-teal-700 rounded px-3 py-1"
            @click="addQuestion(sIdx)"
          >
            + Add Question
          </button>
        </div>
        <div v-else>
          <p class="font-semibold text-lg text-gray-700 mb-2">Text</p>
          <textarea
            v-model="section.textBlankQuestion"
            class="border rounded p-2 w-full mb-2 min-h-[100px]"
            placeholder="Text for blank questions"
          ></textarea>
          <div
            v-for="(blank, bIdx) in section.blankAnswers"
            :key="bIdx"
            class="mb-2"
          >
            <div class="flex gap-4 mb-2 flex-col mt-2">
              <div class="flex flex-row items-center">
                <span class="font-semibold">Answer {{ bIdx + 1 }}</span>
                <button
                  type="button"
                  class="bg-red-100 text-red-700 rounded p-2 ml-2 ml-auto self-end"
                  @click="removeBlankAnswer(sIdx, bIdx)"
                >
                  Delete
                </button>
              </div>
            </div>
            <input
              v-model="blank.blankAnswer"
              class="border rounded p-2 w-full mb-2"
              placeholder="Blank answer text"
            />
          </div>
          <button
            type="button"
            class="bg-teal-100 text-teal-700 rounded px-3 py-1 mb-2"
            @click="addBlankAnswer(sIdx)"
          >
            + Add Blank Answer
          </button>
        </div>
      </div>
      <button
        type="button"
        class="bg-blue-100 text-blue-700 rounded px-3 py-1"
        @click="addSection"
      >
        + Add Section
      </button>
      <button
        type="submit"
        class="mt-4 bg-teal-500 text-white rounded p-2 hover:bg-teal-600 w-[150px] ml-auto self-end cursor-pointer transition-colors duration-200"
      >
        Create Test
      </button>
    </form>
  </div>
</template>
<style>
.is-editor-empty::before {
  color: #6b7280 !important; /* màu xám giống placeholder textarea */
  opacity: 1 !important;
  content: attr(data-placeholder);
  font-family: inherit !important;
  font-size: 1rem !important; /* 16px, giống input/textarea mặc định */
  pointer-events: none;
  float: left;
  height: 0;
  line-height: 1.5;
}
</style>
