# GlimpseTest

## Problem description:
Cameras upload 1 image per second to the system. An ML model runs on every image, the model returns with a list of detected categories (ie. a String array, eg. [“Bottle”, “Glass”, “Cocktail”, …]). Users can see aggregated category results on an interactive chart. Categories are presented on a pie chart and totals by hour are presented on a bar chart. Selecting a category for the pie chart filters the bar chart results by the selected category.

## Design description:
 There is a load balancer, picture saving nodes, ML nodes, a statistics node, UI node, some queues and a storage. 
 The load balancer passes an upload image request to a picture saving node (PSN). 
 The PSN saves the picture into the storage and passes a message containing image ID to ML nodes via a queue A. 
 One of ML nodes receives the message from the queue A, reads the picture from the storage, and passes a message containing detected categories to the statistics node (SN) via queue B. 
 SN reads the queue B and updates counters for categories. 
 UI node contains Angular app and retrieves data to charts. 
 Queues can be based on RabbitMQ or Apache Kafka to be reliable. This design looks flexible in PSN and SN nodes count depending on queues length. 

![alt text](https://github.com/Gdegdevalera/GlimpseTest/blob/master/design.png?raw=true)
