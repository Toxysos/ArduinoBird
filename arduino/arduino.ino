const int trigPin = 8;
const int echoPin = 9;

const byte PIN_LED_R = 3;
const byte PIN_LED_G = 5;
const byte PIN_LED_B = 6;

void setup() {
  Serial.begin(9600);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
    // Initialise les broches
  pinMode(PIN_LED_R, OUTPUT);
  pinMode(PIN_LED_G, OUTPUT);
  pinMode(PIN_LED_B, OUTPUT);
  displayColor(25, 50, 75);
}

void loop() {
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  long duration = pulseIn(echoPin, HIGH);
  int distance = duration * 0.034 / 2;

  Serial.print(distance);

  Serial.println(";");
  delay(300);

   if (Serial.available() > 0) {
    // Read the incoming data
    String colorData = Serial.readStringUntil(';');
    
    // Parse RGB values from the received data
    int commaIndex1 = colorData.indexOf(',');
    int commaIndex2 = colorData.lastIndexOf(',');

    if (commaIndex1 != -1 && commaIndex2 != -1) {
      byte r = colorData.substring(0, commaIndex1).toInt();
      byte g = colorData.substring(commaIndex1 + 1, commaIndex2).toInt();
      byte b = colorData.substring(commaIndex2 + 1).toInt();

      // Change LED color based on received RGB values
      displayColor(r, g, b);
    }
  }
}

void displayColor(byte r, byte g, byte b) {

  // Assigne l'état des broches
  // Version cathode commune
  //analogWrite(PIN_LED_R, r);
  //analogWrite(PIN_LED_G, g);
  //analogWrite(PIN_LED_B, b);

  // Version anode commune
  analogWrite(PIN_LED_R, ~r);
  analogWrite(PIN_LED_G, ~g);
  analogWrite(PIN_LED_B, ~b);
}
