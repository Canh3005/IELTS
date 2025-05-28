<script setup>
import { ref, onBeforeUnmount } from "vue";
import { useEditor, EditorContent } from "@tiptap/vue-3";
import StarterKit from "@tiptap/starter-kit";
import { useRouter } from "vue-router";
const router = useRouter();

const testTitle = ref("");

const task1Content = useEditor({
  extensions: [StarterKit],
  content: "",
});

const task2Content = useEditor({
  extensions: [StarterKit],
  content: "",
});

const task3Content = useEditor({
  extensions: [StarterKit],
  content: "",
});

const handleSubmit = async () => {
  // Kiểm tra xem các trường bắt buộc đã được điền đầy đủ chưa
  if (
    !testTitle.value ||
    !task1Content.value ||
    !task2Content.value ||
    !task3Content.value
  ) {
    alert("Vui lòng điền đầy đủ thông tin!");
    return;
  }
  const payload = {
    title: testTitle.value,
    task1: task1Content.value.getHTML(),
    task2: task2Content.value.getHTML(),
    task3: task3Content.value.getHTML(), // Thêm task3 nếu cần
    // Nếu có thêm trường ảnh, thêm vào đây, ví dụ: task1ImageUrl: ...
  };

  try {
    const res = await fetch(
      `${import.meta.env.VITE_BASE_URL}/api/speaking/create`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      }
    );
    const data = await res.json();
    if (res.ok) {
      alert("Tạo đề Speaking thành công!");
      router.push("/speaking"); // Chuyển hướng đến danh sách đề Writing
      // Reset form hoặc chuyển trang nếu muốn
    } else {
      alert(data.message || "Có lỗi xảy ra!");
    }
  } catch (err) {
    alert("Không thể kết nối server!");
  }
};

// Giải phóng bộ nhớ editor khi component unmount
onBeforeUnmount(() => {
  task1Content.value.destroy();
  task2Content.value.destroy();
});
</script>

<template>
  <div class="bg-gray-100 min-h-screen flex flex-col items-center pt-10">
    <h1 class="text-3xl font-bold text-teal-500 mb-6">
      Create New Speaking Test
    </h1>

    <form
      class="bg-white shadow-lg rounded-lg p-8 w-[800px] flex flex-col gap-6"
      @submit.prevent="handleSubmit"
    >
      <!-- Test Title -->
      <label class="font-semibold text-xl text-gray-700">Test Title</label>
      <input
        type="text"
        placeholder="Test Title"
        class="border p-2 rounded"
        v-model="testTitle"
      />

      <!-- Task 1 -->
      <p class="font-semibold text-xl text-gray-700">Task 1</p>
      <EditorContent
        :editor="task1Content"
        class="border rounded min-h-[110px] p-2"
      />

      <!-- Task 2 -->
      <p class="font-semibold text-xl text-gray-700">Task 2</p>
      <EditorContent
        :editor="task2Content"
        class="border rounded min-h-[110px] p-2"
      />

      <p class="font-semibold text-xl text-gray-700">Task 3</p>
      <EditorContent
        :editor="task3Content"
        class="border rounded min-h-[110px] p-2"
      />

      <button
        type="submit"
        class="mt-4 bg-teal-500 text-white rounded p-2 hover:bg-teal-600 w-[150px] ml-auto self-end cursor-pointer transition-colors duration-200"
      >
        Create
      </button>
    </form>
  </div>
</template>

<style>
.editor {
  min-height: 150px;
  outline: none;
}
.ProseMirror:focus {
  outline: none;
  border: none;
  box-shadow: none;
}
</style>
