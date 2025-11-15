int buttonPin1 = 7, buttonPin2 = 8, buttonPin3 = 6;

int buttonState1 = LOW, buttonState2 = LOW, buttonState3 = LOW;
int lastReading1 = LOW, lastReading2 = LOW, lastReading3 = LOW;

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

void checkButton(int pin, int care, int &lastReading, int &buttonState, unsigned long &lastTime)
{
  int reading = digitalRead(pin);
  if (reading != lastReading) {
    lastTime = millis();
  }
  if ((millis() - lastTime) > debounceDelay) {
    if (reading != buttonState) {
      buttonState = reading;
      if (buttonState == HIGH) {
        Serial.println("Button " + (String)care + " pressed\n");
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
