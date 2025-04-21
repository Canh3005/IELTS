<script setup>
import { ref, watchEffect, onMounted } from "vue";
import { useRouter } from "vue-router";

const isLoggedIn = ref(!!localStorage.getItem("token")); // Kiểm tra xem người dùng đã đăng nhập hay chưa
const router = useRouter();
const username = ref(localStorage.getItem("username"));

watchEffect(() => {
  isLoggedIn.value = !!localStorage.getItem("token");
  username.value = localStorage.getItem("username"); // Lấy username từ localStorage
});

const logout = () => {
  isLoggedIn.value = false; // Đặt trạng thái đăng xuất
  localStorage.removeItem("token"); // Xóa token
  localStorage.removeItem("username"); // Xóa username
  router.push("/"); // Chuyển hướng về trang đăng nhập
};

// Kiểm tra thời hạn của token
const checkTokenExpiration = () => {
  const token = localStorage.getItem("token");
  console.log(token);
  if (token) {
    const payload = JSON.parse(atob(token.split(".")[1])); // Giải mã payload của JWT
    const expiration = payload.exp * 1000; // Chuyển thời gian hết hạn sang milliseconds
    const now = Date.now();

    console.log(expiration, now);

    if (now >= expiration) {
      logout(); // Token đã hết hạn, tự động đăng xuất
    } else {
      // Đặt timeout để tự động đăng xuất khi token hết hạn
      setTimeout(logout, expiration - now);
    }
  }
};

onMounted(() => {
  checkTokenExpiration(); // Kiểm tra token khi component được mount
});
</script>

<template>
  <nav class="flex flex-row bg-teal-500 text-white w-full">
    <router-link to="/" class="w-1/8"
      ><img src="../assets/Logo.png" alt="Logo" class="h-15"
    /></router-link>
    <div class="container mx-auto flex justify-between items-center w-2/3">
      <ul class="flex space-x-8 w-1/2 mr-auto font-bold items-center">
        <li>
          <router-link
            to="/"
            class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
            >Home</router-link
          >
        </li>
        <li>
          <router-link
            to="/listening"
            class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
            >Listening</router-link
          >
        </li>
        <li>
          <router-link
            to="/reading"
            class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
            >Reading</router-link
          >
        </li>
        <li>
          <router-link
            to="/writing"
            class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
            >Writing</router-link
          >
        </li>
        <li>
          <router-link
            to="/speaking"
            class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
            >Speaking</router-link
          >
        </li>
        <li>
          <router-link
            to="/exam"
            class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
            >Test</router-link
          >
        </li>
      </ul>

      <template v-if="isLoggedIn">
        <div
          class="flex flex-row items-center space-x-4 w-1/4 justify-center font-bold h-full"
        >
          <img
            src="../assets/avt.png"
            alt="avatar"
            class="w-10 h-10 rounded-full"
          />
          <div class="relative group hover:cursor-pointer h-full">
            <button
              class="text-white font-bold inline-flex items-center p-2 border border-[white] hover:bg-teal-600 mt-2 min-w-[100px] justify-center"
            >
              <p class="">{{ username }}</p>
              <img
                src="../assets/caret.png"
                alt="arrow"
                class="w-3 h-3 mt-1.1 ml-2"
              />
            </button>
            <ul
              class="absolute right-0 bg-white text-teal-500 shadow-lg hidden group-hover:block w-full"
            >
              <li class="px-4 py-2 hover:bg-teal-100">
                <router-link to="/profile">Profile</router-link>
              </li>
              <li class="px-4 py-2 hover:bg-teal-100">
                <router-link to="/progress">Progress</router-link>
              </li>
              <li
                class="px-4 py-2 hover:bg-teal-100 cursor-pointer"
                @click="logout"
              >
                Log Out
              </li>
            </ul>
          </div>
        </div>
      </template>

      <template v-else>
        <ul class="flex space-x-4 w-1/4 justify-center font-bold items-center">
          <li>
            <router-link
              to="/register"
              class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
              >Register</router-link
            >
          </li>
          <li>
            <router-link
              to="/login"
              class="text-white !no-underline hover:after:bg-[white] hover:scale-110 transition-transform duration-200 inline-block"
              >Login</router-link
            >
          </li>
        </ul>
      </template>
    </div>
  </nav>
</template>
