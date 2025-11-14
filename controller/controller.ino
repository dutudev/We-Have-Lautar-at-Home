int buttonPin1 = 7, buttonPin2 = 8, buttonPin3 = 6;

// Current and previous STABLE states
int buttonState1 = LOW, buttonState2 = LOW, buttonState3 = LOW;
int lastReading1 = LOW, lastReading2 = LOW, lastReading3 = LOW;

// Debounce timers
unsigned long lastDebounceTime1 = 0;
unsigned long lastDebounceTime2 = 0;
unsigned long lastDebounceTime3 = 0;

unsigned long debounceDelay = 50;

void setup() {
  pinMode(buttonPin1, INPUT);
  pinMode(buttonPin2, INPUT);
  pinMode(buttonPin3, INPUT);

  Serial.begin(9600);
}

// Generic debounced press check
void checkButton(int pin, int care,
                 int &lastReading,
                 int &buttonState,
                 unsigned long &lastTime)
{
  int reading = digitalRead(pin);

  // If reading changed, reset the debounce timer
  if (reading != lastReading) {
    lastTime = millis();
  }

  // If stable for longer than debounceDelay, accept it
  if ((millis() - lastTime) > debounceDelay) {
    if (reading != buttonState) {
      buttonState = reading;

      // Button pressed (LOW->HIGH)
      if (buttonState == HIGH) {
        Serial.println("Button " + (String)care + " pressed");
      }
    }
  }

  lastReading = reading;
}

void loop() {
  checkButton(buttonPin1, 1, lastReading1, buttonState1, lastDebounceTime1);
  checkButton(buttonPin2, 2, lastReading2, buttonState2, lastDebounceTime2);
  checkButton(buttonPin3, 3, lastReading3, buttonState3, lastDebounceTime3);
}
