<script setup>
import { onMounted } from "vue";
import { ref } from "vue";
import axios from "axios";
import Navbar from "../components/Navbar.vue"; // Import Navbar component
import Footer from "../components/Footer.vue"; // Import Footer component
import { useRouter } from "vue-router";
const router = useRouter();

const username = ref("");
const email = ref("");
const password = ref("");
const message = ref("");
const isSuccess = ref(false);
const isPasswordVisible = ref(false);

onMounted(() => {
  document.title = "Login";
});

const login = async () => {
  if (!email.value || !password.value) {
    message.value = "Please fill in the information completely!";
    isSuccess.value = false;
    return;
  }
  try {
    const response = await axios.post("http://localhost:5004/api/auth/login", {
      email: email.value,
      password: password.value,
    });
    message.value = response.data.message;
    if (response.data.message === "Login successful.") {
      isSuccess.value = true;
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("username", response.data.username);
      localStorage.setItem("userid", response.data.userId);
      alert("Login successful! Redirecting to home page...");
      window.location.href = "/";
    } else {
      isSuccess.value = false;
    }
  } catch (error) {
    message.value = error.response?.data?.message || "Login error!";
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
          src="../assets/login/Login.jpg"
          alt="Login"
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
          <button class="p-1 bg-teal-500 rounded-[33px] w-1/2 text-white">
            Login
          </button>
          <button
            href="/register"
            class="p-1 bg-teal-400 rounded-[33px] w-1/2 text-white hover:bg-teal-500"
          >
            <router-link to="/register">Register</router-link>
          </button>
        </div>
        <p class="mt-4 mb-2 text-gray-500">
          Please enter the information to login
        </p>
        <form class="bg-white p-2 w-[80%]">
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
          <div class="flex justify-end mb-3 mr-2">
            <a
              href="#"
              class="text-gray-500 hover:text-teal-500 hover:underline"
              >Forgot Password ?</a
            >
          </div>
          <div
            v-if="message"
            :class="isSuccess ? 'text-green-500' : 'text-red-500'"
            class="mt-4 text-center"
          >
            {{ message }}
          </div>
          <div class="flex justify-center mt-2">
            <button
              @click="login"
              type="button"
              class="w-1/2 bg-teal-400 text-white py-2 rounded-[33px] hover:bg-teal-500 cursor-pointer"
            >
              Login
            </button>
          </div>
        </form>
      </div>
    </div>
    <!-- Add Footer component here -->
    <Footer />
  </div>
</template>
