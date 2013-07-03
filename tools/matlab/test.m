%----------------------------------------------
%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_1\20130605_2004_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_1\20130605_2004_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_1\20130605_2004_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_2\20130607_1729_arousal.csv', 3);
[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_2\20130607_1729_gsr.csv', 3);
[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_2\20130607_1729_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_3\20130611_1458_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_3\20130611_1458_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_3\20130611_1458_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_4\20130612_1322_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_4\20130612_1322_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_4\20130612_1322_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_5\20130612_1515_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_5\20130612_1455_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_5\20130612_1455_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_6\20130612_1703_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_6\20130612_1619_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_6\20130612_1619_metrics.csv', 16);

%----------------------------------------------
%time_millisecond
%arousal
%player_speed
%zombie_speed
%fog_start_dist
%fog_end_dist
%current_round
%zombie_threshold
%zombie_increase_power
%max_zombie_alive
%number_of_alive_zombies
%number_of_killed_zombies
%grenade_regen_delay
%medic_regen_delay
%calibrating
%adaptation_condition
%----------------------------------------------

%plot(gsr_v{1}, (gsr_v{2} * 1000) + 800 , 'b.'); hold on;
plot(gsr_v{1}, smooth(gsr_v{1}, (gsr_v{2} * 1000) + 800, 0.1, 'loess'), 'r-'); hold on; 
%plot(gsr_v{1}, gsr_v{2}, 'b.', gsr_v{1}, smooth(gsr_v{1}, (gsr_v{2} * 1000) + 800, 0.1, 'loess'),'r-')
plot(metrics_v{1}, metrics_v{15} * 100 + 25, 'k-'); hold on; % calibration
plot(metrics_v{1}, metrics_v{16} * 20 + 50, 'c-'); hold on; % adaptation
%plot(bvp_v{1}, bvp_v{3}, 'c-'); %hold on;
%plot(hr_v{1}, hr_v{3}, 'g-'); hold on; 
%plot(arousal_v{1}, arousal_v{3}, 'r-'); hold on; 
%plot(metrics_v{1}, metrics_v{2} * 100, 'b-'); hold on; % arousal
plot(metrics_v{1}, (metrics_v{3}) * 100 / 2.3, 'y-'); %hold on; % player speed
plot(metrics_v{1}, (metrics_v{4} - 1 ) * 100 / 3, 'g-'); %hold on; % zombie speed
plot(metrics_v{1}, (metrics_v{5} - 70) * 100 / 380, 'm-'); %hold on; % fog distant
legend('GSR Data', 'Calibration', 'Condition', 'Player Speed', 'Zombie Speed', 'Fog Distant', 'Location','NW')
%----------------------------------------------
xlabel('time');
ylabel('signal');
%grid on;
%----------------------------------------------
