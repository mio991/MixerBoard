int chanels[] = {A4, A3, A2, A1, A0};
size_t chanel_count = sizeof(chanels) / sizeof(int);

int refresh_delay = 100;

void setup() {
  Serial.begin(9600);
}

void loop() {
    for(size_t i = 0; i < chanel_count; i++){
      if(i > 0){
        Serial.print("; ");
      }
      Serial.print(analogRead(chanels[i]));
    }
    Serial.println();

    delay(refresh_delay);
}
