<script setup>
import { onMounted, watch } from "vue";
import { ref } from "vue";
import axios from "axios";
import Navbar from "../components/Navbar.vue"; // Import Navbar component
import Footer from "../components/Footer.vue"; // Import Footer component

const isRegistering = ref(true); // Trạng thái mặc định là đăng ký
const username = ref("");
const email = ref("");
const password = ref("");
const isChecked = ref(false);
const message = ref("");
const isSuccess = ref(false); // Trạng thái thành công hay thất bại
const isPasswordVisible = ref(false); // Trạng thái hiển thị mật khẩu
const usernameError = ref(""); // Lưu thông báo lỗi cho username

onMounted(() => {
  document.title = "Register";
});

// Theo dõi sự thay đổi của username
watch(username, (newValue) => {
  if (newValue && !/^[A-Z]/.test(newValue)) {
    usernameError.value = "The first letter of the username must be uppercase.";
  } else {
    usernameError.value = ""; // Xóa lỗi nếu hợp lệ
  }
});

const register = async () => {
  if (!username.value || !email.value || !password.value) {
    message.value = "Please fill in the information completely!";
    isSuccess.value = false;
    return;
  }
  if (!isChecked.value) {
    message.value = "You need to agree to the terms and conditions!";
    isSuccess.value = false;
    return;
  }
  try {
    const response = await axios.post(
      `${import.meta.env.VITE_BASE_URL}/api/auth/register`,
      {
        username: username.value,
        email: email.value,
        password: password.value,
      }
    );
    message.value = response.data.message;
    if (response.data.message === "Register successfully.") {
      isSuccess.value = true;
      localStorage.setItem("username", username.value); // Lưu username vào localStorage
    }
  } catch (error) {
    message.value = error.response?.data?.message || "Register error!";
    isSuccess.value = false;
  }
};
</script>

<template>
  <div class="flex flex-col items-center bg-gray-100">
    <!-- Add Navbar component here -->
    <div
      class="flex flex-row <p></p>items-center justify-center m-10 mt-30 rounded-[33px] shadow-lg max-w-[2000px] w-[1000px] h-[600px] bg-white"
    >
      <div class="w-1/2 p-4">
        <img
          src="../assets/Register.jpg"
          alt="Register"
          class="rounded-[33px] h-full object-cover"
        />
      </div>
      <div class="w-1/2 p-4 flex flex-col items-center rounded-lg">
        <p class="mt-4 mb-4 text-3xl font-bold text-teal-500">
          Welcome to IELTS Talent
        </p>
        <div
          class="bg-teal-400 flex justify-center space-x-4 w-1/2 rounded-[33px] p-1.5"
        >
          <button
            class="p-1 bg-teal-400 rounded-[33px] w-1/2 text-white hover:bg-teal-500"
          >
            <router-link to="/login">Login</router-link>
          </button>
          <button class="p-1 bg-teal-500 rounded-[33px] w-1/2 text-white">
            Register
          </button>
        </div>
        <p class="mt-4 mb-2 text-gray-500">
          Please enter the information to register
        </p>
        <form class="bg-white p-2 w-[80%]">
          <p class="mb-1">Username<span class="text-[red] ml-1">*</span></p>
          <input
            v-model="username"
            type="text"
            placeholder="Enter your username"
            class="w-full p-2 mb-4 border border-gray-300 rounded-[33px] border-teal-400 pl-4"
            required
          />
          <!-- Hiển thị lỗi nếu username không hợp lệ -->
          <p v-if="usernameError" class="text-red-500 text-sm mb-4">
            {{ usernameError }}
          </p>
          <p class="mb-1">
            Email Address<span class="text-[red] ml-1">*</span>
          </p>
          <input
            v-model="email"
            type="email"
            placeholder="Enter your email address"
            class="w-full p-2 mb-4 border border-gray-300 rounded-[33px] border-teal-400 pl-4"
            required
          />
          <p class="mb-1">Password<span class="text-[red] ml-1">*</span></p>
          <div class="relative w-full">
            <input
              v-model="password"
              :type="isPasswordVisible ? 'text' : 'password'"
              placeholder="Enter your password"
              class="w-full p-2 mb-4 border border-gray-300 rounded-[33px] border-teal-400 pl-4"
              required
            />
            <button
              type="button"
              @click="isPasswordVisible = !isPasswordVisible"
              class="absolute right-4 top-2.5 text-gray-500 hover:text-teal-500"
            >
              <span v-if="isPasswordVisible"
                ><img src="../assets/login/eye.png" alt="" class="w-5 h-5"
              /></span>
              <span v-else
                ><img src="../assets/login/hidden.png" alt="" class="w-5 h-5"
              /></span>
            </button>
          </div>
          <div class="ml-1 mb-3">
            <input
              type="checkbox"
              v-model="isChecked"
              class="mr-2 accent-teal-200"
              required
            />
            <label for="checkbox">I agree to the terms and conditions</label>
          </div>
          <p
            v-if="message"
            :class="isSuccess ? 'text-green-500' : 'text-red-500'"
          >
            {{ message }}
            <span v-if="isSuccess">
              <router-link
                to="/login"
                class="text-blue-500 underline hover:text-teal-500"
              >
                Login now!
              </router-link>
            </span>
          </p>
          <div class="flex justify-center mt-3">
            <button
              @click="register"
              type="button"
              class="w-1/2 bg-teal-400 text-white py-2 rounded-[33px] hover:bg-teal-500 cursor-pointer"
            >
              Register
            </button>
          </div>
        </form>
      </div>
    </div>
    <Footer />
  </div>
</template>
