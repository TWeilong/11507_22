package com.example.home_work_2
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.unit.dp
import coil.compose.AsyncImage
import com.example.home_work_2.ui.theme.Home_Work_2Theme
import kotlin.random.Random

class MainActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()

        setContent {
                Home_Work_2Theme {
                AuthorizationApp()
            }
        }
    }
}

@Composable
fun AuthorizationApp() {

    val generatedPassword = remember {
        generatePassword()
    }

    var isSecondScreen by remember {
        mutableStateOf(false)
    }

    var email by remember {
        mutableStateOf("")
    }

    var enteredPassword by remember {
        mutableStateOf("")
    }

    var emailError by remember {
        mutableStateOf<String?>(null)
    }

    var passwordError by remember {
        mutableStateOf<String?>(null)
    }

    fun goToSecondScreen() {

        emailError = when {
            email.isBlank() -> {
                "Введите email"
            }

            !android.util.Patterns.EMAIL_ADDRESS
                .matcher(email)
                .matches() -> {
                "Введите корректный email"
            }

            else -> {
                null
            }
        }

        passwordError = when {
            enteredPassword.isBlank() -> {
                "Введите пароль"
            }

            enteredPassword.length < 8 -> {
                "Пароль должен содержать минимум 8 символов"
            }

            !enteredPassword.any { it.isLetter() } -> {
                "Пароль должен содержать латинские буквы"
            }

            !enteredPassword.any { it.isDigit() } -> {
                "Пароль должен содержать цифры"
            }

            enteredPassword.any { it.code > 127 } -> {
                "Используйте только латинские символы"
            }

            else -> {
                null
            }
        }

        if (emailError == null && passwordError == null) {
            isSecondScreen = true
        }
    }

    Surface(
        modifier = Modifier.fillMaxSize()
    ) {
        if (isSecondScreen) {
            SecondScreen(
                generatedPassword = generatedPassword,
                enteredPassword = enteredPassword,
                onBackClick = {
                    isSecondScreen = false
                }
            )
        } else {
            FirstScreen(
                email = email,
                onEmailChange = {
                    email = it
                },
                emailError = emailError,
                password = enteredPassword,
                onPasswordChange = {
                    enteredPassword = it
                },
                passwordError = passwordError,
                onNextClick = {
                    goToSecondScreen()
                }
            )
        }
    }
}

@Composable
fun FirstScreen(
    email: String,
    onEmailChange: (String) -> Unit,
    emailError: String?,
    password: String,
    onPasswordChange: (String) -> Unit,
    passwordError: String?,
    onNextClick: () -> Unit
) {
    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(24.dp),
        verticalArrangement = Arrangement.Center
    ) {
        Text(
            text = "Авторизация",
            style = MaterialTheme.typography.headlineMedium
        )

        Spacer(modifier = Modifier.height(24.dp))

        OutlinedTextField(
            value = email,
            onValueChange = onEmailChange,
            modifier = Modifier.fillMaxWidth(),
            label = {
                Text("Email")
            },
            singleLine = true,
            isError = emailError != null,
            supportingText = {
                if (emailError != null) {
                    Text(emailError)
                }
            },
        )

        Spacer(modifier = Modifier.height(12.dp))

        OutlinedTextField(
            value = password,
            onValueChange = onPasswordChange,
            modifier = Modifier.fillMaxWidth(),
            label = {
                Text("Пароль")
            },
            singleLine = true,
            isError = passwordError != null,
            supportingText = {
                if (passwordError != null) {
                    Text(passwordError)
                }
            },
            visualTransformation = PasswordVisualTransformation(),
        )

        Spacer(modifier = Modifier.height(20.dp))

        Button(
            onClick = onNextClick,
            modifier = Modifier.fillMaxWidth()
        ) {
            Text("Далее")
        }
    }
}

@Composable
fun SecondScreen(
    generatedPassword: String,
    enteredPassword: String,
    onBackClick: () -> Unit
) {
    var showPassword by remember {
        mutableStateOf(false)
    }

    val passwordsMatch = generatedPassword == enteredPassword

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(24.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        AsyncImage(
            model = if (passwordsMatch) {
                "https://placehold.co/600x400/4CAF50/FFFFFF?text=SUCCESS"
            } else {
                "https://placehold.co/600x400/F44336/FFFFFF?text=ERROR"
            },
            contentDescription = "Результат авторизации",
            modifier = Modifier.size(240.dp)
        )

        Spacer(modifier = Modifier.height(20.dp))

        Text(
            text = if (passwordsMatch) {
                "Пароль совпал"
            } else {
                "Пароль не совпал"
            },
            style = MaterialTheme.typography.titleLarge
        )

        Spacer(modifier = Modifier.height(20.dp))

        OutlinedTextField(
            value = generatedPassword,
            onValueChange = {},
            modifier = Modifier.fillMaxWidth(),
            label = {
                Text("Сгенерированный пароль")
            },
            readOnly = true,
            singleLine = true,
            visualTransformation = if (showPassword) {
                VisualTransformation.None
            } else {
                PasswordVisualTransformation()
            },
            trailingIcon = {
                TextButton(
                    onClick = {
                        showPassword = !showPassword
                    }
                ) {
                    Text(
                        text = if (showPassword) {
                            "Скрыть"
                        } else {
                            "Показать"
                        }
                    )
                }
            }
        )

        Spacer(modifier = Modifier.height(20.dp))

        Button(
            onClick = onBackClick,
            modifier = Modifier.fillMaxWidth()
        ) {
            Text("Вернуться")
        }
    }
}

fun generatePassword(): String {
    val lettersAndNumbers =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"

    val password = CharArray(10) {
        lettersAndNumbers[Random.nextInt(lettersAndNumbers.length)]
    }

    password[0] = ('A'..'Z').random()
    password[1] = ('0'..'9').random()

    return password.concatToString()
}
