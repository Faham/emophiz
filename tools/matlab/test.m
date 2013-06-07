[gsr_h, gsr_v] = readCSV('D:\faham\emophiz\emophiz\logs\20130607_1649_gsr.csv', 3)
[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\20130607_1649_metrics.csv', 16)
%time_millisecond,arousal,player_speed,zombie_speed,fog_start_dist,fog_end_dist,current_round,zombie_threshold,zombie_increase_power,max_zombie_alive,number_of_alive_zombies,number_of_killed_zombies,grenade_regen_delay,medic_regen_delay,calibrating,adaptation_condition

%plot(bvp_v{1}, bvp_v{3}, 'c-');
%hold on;
%plot(hr_v{1}, hr_v{3}, 'g-');
%hold on;
plot(gsr_v{1}, smooth(gsr_v{1}, (gsr_v{2} * 1000) + 800, 0.1, 'loess'), 'r-');
hold on;
%plot(arousal_v{1}, arousal_v{3}, 'b-');
%hold on;
%plot(metrics_v{1}, metrics_v{2} * 100, 'b-'); % arousal
hold on;
plot(metrics_v{1}, metrics_v{15} * 100 + 25, 'k-'); % calibration
%hold on;
plot(metrics_v{1}, metrics_v{16} * 20 + 50, 'c-'); % adaptation
%hold on;
%plot(metrics_v{1}, (metrics_v{2} - 0.7) * 100 / 2.3, 'm-'); % player speed
%hold on;
%plot(metrics_v{1}, (metrics_v{3} - 1 ) * 100 / 3, 'm-'); % zombie speed
%hold on;
%plot(metrics_v{1}, (metrics_v{4} - 70) * 100 / 380, 'm-'); % fog distant
xlabel('time');
ylabel('signal');
%grid on;
